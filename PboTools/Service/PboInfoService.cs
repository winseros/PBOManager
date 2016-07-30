using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using PboTools.Domain;
using Util;

namespace PboTools.Service
{
	public class PboInfoService : IPboInfoService
	{
		private static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

		public PboInfo ReadPboInfo(PboBinaryReader reader)
		{
            Assert.NotNull(reader, nameof(reader));

			var info = new PboInfo();
			info.FileRecords = new Collection<PboHeaderEntry>();

			PboHeaderEntry signature = this.ReadSingleHeaderEntry(reader);
			if (signature.PackingMethod == PboPackingMethod.Product)
			{
				info.Signature = signature;
				info.HeaderExtensions = this.ReadHeaderExtensions(reader);
			}
			else
			{
				info.FileRecords.Add(signature);
				info.HeaderExtensions = new NameValueCollection();
			}

			this.ReadRemainingHeaderEntries(reader, info.FileRecords);			

			this.SetPboOffsets(info, reader);

			info.Checksum = this.ReadPboChecksum(reader);

			return info;
		}	

		private PboHeaderEntry ReadSingleHeaderEntry(PboBinaryReader reader)
		{
			var entry = new PboHeaderEntry();

			entry.FileName = reader.ReadNullTerminatedString();
			entry.PackingMethod = (PboPackingMethod) reader.ReadInt32();
			entry.OriginalSize = reader.ReadInt32();
			entry.Reserved = reader.ReadInt32();
			entry.TimeStamp = reader.ReadInt32();
			entry.DataSize = reader.ReadInt32();

			return entry;
		}

		private void ReadRemainingHeaderEntries(PboBinaryReader reader, IList<PboHeaderEntry> existingEntries)
		{
			PboHeaderEntry header = this.ReadSingleHeaderEntry(reader);			
			while (!header.IsBoundary)
			{
				existingEntries.Add(header);
				header = this.ReadSingleHeaderEntry(reader);
			}
		}

		private void SetPboOffsets(PboInfo info, PboBinaryReader reader)
		{			
			info.DataBlockStart = reader.BaseStream.Position;
			long currentOffset = info.DataBlockStart;
			
			foreach (var headerEntry in info.FileRecords)
			{
				headerEntry.DataOffset = currentOffset;
				currentOffset += headerEntry.DataSize;
			}
		}

		private NameValueCollection ReadHeaderExtensions(PboBinaryReader reader)
		{
			var result = new NameValueCollection();

			string name;
			while ((name = reader.ReadNullTerminatedString()) != string.Empty)
			{
				var value = reader.ReadNullTerminatedString();
				result.Add(name, value);
			}

			return result;
		}

		private byte[] ReadPboChecksum(PboBinaryReader reader)
		{
			const byte checkSumLength = 21;
			Stream stream = reader.BaseStream;

			if (stream.Length - stream.Position < checkSumLength)
				return null;

			stream.Position = stream.Length - checkSumLength;
			byte[] result = reader.ReadBytes(checkSumLength);

			return result;
		}

		public void WritePboInfo(PboBinaryWriter writer, PboInfo info)
		{
            Assert.NotNull(writer, nameof(writer));
            Assert.NotNull(info, nameof(info));

            if (info.Signature != null)
			{
				this.WriteSingleEntry(writer, info.Signature);
				this.WriteHeaderExtensions(writer, info.HeaderExtensions);
			}
			this.WriteHeaderEntries(writer, info);
		}		

		private void WriteSingleEntry(PboBinaryWriter writer, PboHeaderEntry entry)
		{
			writer.WriteNullTerminatedString(entry.FileName);
			writer.Write((int)entry.PackingMethod);
			writer.Write(entry.OriginalSize);
			writer.Write(entry.Reserved);
			writer.Write(entry.TimeStamp);
			writer.Write(entry.DataSize);			
		}

		private void WriteHeaderEntries(PboBinaryWriter writer, PboInfo info)
		{
			foreach (PboHeaderEntry headerEntry in info.FileRecords)
				this.WriteSingleEntry(writer, headerEntry);

			var boundaryEntry = PboHeaderEntry.CreateBoundary();
			this.WriteSingleEntry(writer, boundaryEntry);
		}

		private void WriteHeaderExtensions(PboBinaryWriter writer, NameValueCollection extensions)
		{
			foreach (string extension in extensions)
			{
				writer.WriteNullTerminatedString(extension);
				writer.WriteNullTerminatedString(extensions[extension]);
			}
			writer.Write((byte)0);
		}

		public PboInfo CollectPboInfo(DirectoryInfo directory)
		{
            Assert.NotNull(directory, nameof(directory));

            var info = new PboInfo();
			info.Signature = PboHeaderEntry.CreateSignature();
			info.HeaderExtensions = new NameValueCollection();			

			this.CollectFileInfo(directory, info);

			return info;
		}

		private void CollectFileInfo(DirectoryInfo directory, PboInfo pboInfo)
		{
			FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
			pboInfo.FileRecords = new List<PboHeaderEntry>(files.Length);

			for (var i = 0; i < files.Length; i++)
			{
				PboHeaderEntry entry = this.GetHeaderEntry(files[i], directory);
				pboInfo.FileRecords.Add(entry);
			}
		}

		private PboHeaderEntry GetHeaderEntry(FileInfo file, DirectoryInfo directory)
		{			
			var result = new PboHeaderEntry();

			result.FileName = file.FullName.Substring(directory.FullName.Length + 1, file.FullName.Length - directory.FullName.Length - 1);
			result.PackingMethod = PboPackingMethod.Uncompressed;
			result.OriginalSize = (int) file.Length;
			result.TimeStamp = (int)file.LastWriteTimeUtc.Subtract(Date1970).TotalSeconds;
			result.DataSize = result.OriginalSize;

			return result;
		}
	}
}