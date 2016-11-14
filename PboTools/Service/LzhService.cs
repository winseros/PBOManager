using System.IO;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace PboTools.Service
{
    public class LzhService : ILzhService
    {
        private const int PacketFormatUncompressed = 1;

        public async Task Decompress(Stream source, Stream dest, long targetLength)
        {
            Assert.NotNull(source, nameof(source));
            Assert.NotNull(dest, nameof(dest));
            Assert.Greater(targetLength, 0, nameof(targetLength));

            using (var reader = new BinaryReader(source, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(dest, Encoding.UTF8, true))
            {
                var buffer = new byte[18];
                long noOfBytes = dest.Position + targetLength;
                while (dest.Position < noOfBytes && source.CanRead)
                {
                    byte format = reader.ReadByte();
                    byte i = 0;
                    while (i < 8 && dest.Position < noOfBytes && source.Position < source.Length - 2)
                    {
                        int bit = format >> i & 0x01;
                        this.ProcessBlock(reader, writer, dest, bit, buffer);
                        await Task.Yield();
                        i++;
                    }
                }
            }
        }

        private void ProcessBlock(BinaryReader reader, BinaryWriter writer, Stream dest, int format, byte[] buffer)
        {
            if (format == LzhService.PacketFormatUncompressed)
            {
                byte data = reader.ReadByte();
                writer.Write(data);
            }
            else
            {
                short pointer = reader.ReadInt16();
                long rpos = dest.Position - ((pointer & 0x00FF) + ((pointer & 0xF000) >> 4));
                if (rpos < 0) rpos = 0;
                int rlen = ((pointer & 0x0F00) >> 8) + 3;

                if (rpos + rlen < 0)
                {
                    for (var i = 0; i < rlen; i++)
                        writer.Write(0x20);
                }
                else
                {
                    int bytesToCopy = rpos + rlen > dest.Position ? (int)(dest.Position - rpos) : rlen;
                    if (bytesToCopy > 0)
                    {
                        dest.Seek(rpos, SeekOrigin.Begin);
                        dest.Read(buffer, 0, bytesToCopy);
                        dest.Seek(0, SeekOrigin.End);
                    
                        int bytesLeft = rlen;
                        while (bytesLeft >= bytesToCopy)
                        {
                            writer.Write(buffer, 0, bytesToCopy);
                            bytesLeft -= bytesToCopy;
                        }
                        for (int j = 0; j < bytesLeft; j++)
                        {
                            writer.Write(buffer[j]);
                        }
                    }
                }
            }
        }
    }
}