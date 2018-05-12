using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using PboTools.Domain;
using Util;

namespace PboTools.Service
{
    public class PboInfoService : IPboInfoService
    {
        private readonly ITimestampService timestampService;

        public PboInfoService(ITimestampService timestampService)
        {
            this.timestampService = timestampService;
        }

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

            PboHeaderEntry lastEntry = info.FileRecords.LastOrDefault();
            info.Checksum = this.ReadPboChecksum(reader, lastEntry);

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

            info.DataBlockEnd = currentOffset;
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

        private byte[] ReadPboChecksum(PboBinaryReader reader, PboHeaderEntry lastEntry)
        {
            const byte checkSumLength = 20;
            Stream stream = reader.BaseStream;

            if (lastEntry == null || stream.Length - lastEntry.DataOffset - lastEntry.DataSize < checkSumLength)
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
            writer.Write((int) entry.PackingMethod);
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
            writer.Write((byte) 0);
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
            string fn = directory.FullName;
            int offset = fn[fn.Length - 1] == Path.DirectorySeparatorChar ? 0 : 1;
            var result = new PboHeaderEntry
            {
                FileName = file.FullName.Substring(fn.Length + offset, file.FullName.Length - fn.Length - offset)
            };

            this.InflateEntry(result, file);

            return result;
        }

        private void InflateEntry(PboHeaderEntry entry, FileInfo file)
        {
            entry.PackingMethod = PboPackingMethod.Uncompressed;
            entry.OriginalSize = (int)file.Length;
            entry.TimeStamp = this.timestampService.GetTimestamp(file.FullName);
            entry.DataSize = entry.OriginalSize;
        }

        public PboHeaderEntry CollectEntry(string filePath, string entryPath)
        {
            Assert.NotNull(filePath, nameof(filePath));
            Assert.NotNull(entryPath, nameof(entryPath));

            var result = new PboHeaderEntry
            {
                FileName = Path.Combine(entryPath, Path.GetFileName(filePath))
            };

            this.InflateEntry(result, new FileInfo(filePath));

            return result;
        }
    }
}