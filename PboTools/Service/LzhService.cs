using System.IO;
using System.Text;
using Util;

namespace PboTools.Service
{
	public class LzhService : ILzhService
	{
		public void Decompress(Stream source, Stream dest, long targetLength)
		{
			Assert.NotNull(source, nameof(source));
			Assert.NotNull(dest, nameof(dest));
			Assert.Greater(targetLength, 0, nameof(targetLength));					

			var reader = new BinaryReader(source, Encoding.UTF8);

			var writer = new BinaryWriter(dest);
			long noOfBytes = dest.Position + targetLength;
			while (dest.Position < noOfBytes && source.Position < source.Length)
			{
				var format = reader.ReadByte();
				var i = 0;
				while (i < 8 && dest.Position < noOfBytes && source.Position < source.Length - 2)
				{
					var bit = format >> i & 0x01;
					this.ProcessBlock(reader, writer, bit);
					i++;
				}
			}
		}

		public void ProcessBlock(BinaryReader reader, BinaryWriter writer, int format)
		{			
			if (format == 1)
			{
				var data = reader.ReadByte();
				writer.Write(data);
			}
			else
			{
				var pointer = reader.ReadInt16();
				var rpos = writer.BaseStream.Position - ((pointer & 0x00FF) + ((pointer & 0xF000) >> 4));
				var rlen = ((pointer & 0x0F00) >> 8) + 3;

				if (rpos + rlen < 0)
				{
					for (var i = 0; i < rlen; i++)
						writer.Write(0x20);
				}
				else
				{
					var bytesToCopy = (rpos + rlen > writer.BaseStream.Position) ? writer.BaseStream.Position - rpos : rlen;

					writer.BaseStream.Seek(rpos, SeekOrigin.Begin);

					var copy = new byte[bytesToCopy];
					writer.BaseStream.Read(copy, 0, copy.Length);

					writer.BaseStream.Seek(writer.BaseStream.Length, SeekOrigin.Begin);

					if (copy.Length > 0)
					{
						var bytesLeft = rlen;
						while (bytesLeft >= copy.Length)
						{
							writer.Write(copy);
							bytesLeft -= copy.Length;
						}
						var i = 0;
						while (bytesLeft > 0)
						{
							writer.Write(copy[i]);
							bytesLeft--;
							i++;
						}
					}
				}				
			}
		}
	}
}