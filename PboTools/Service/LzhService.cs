using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace PboTools.Service
{
    public class LzhService : ILzhService
    {
        private const int PacketFormatUncompressed = 1;
        private const byte Space = 0x20;

        public async Task<bool> Decompress(Stream source, Stream dest, long targetLength)
        {
            Assert.NotNull(source, nameof(source));
            Assert.NotNull(dest, nameof(dest));
            Assert.Greater(targetLength, 0, nameof(targetLength));

            using (var reader = new BinaryReader(source, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(dest, Encoding.UTF8, true))
            {
                var ctx = new ProcessContext {Reader = reader, Writer = writer, Dest = dest};               
                long noOfBytes = dest.Position + targetLength;
                while (dest.Position < noOfBytes && source.CanRead)
                {
                    byte format = reader.ReadByte();
                    for (byte i = 0; i < 8 && dest.Position < noOfBytes && source.Position < source.Length - 2; i++)
                    {
                        ctx.Format = format >> i & 0x01;
                        this.ProcessBlock(ctx);
                        await Task.Yield();
                    }
                }
                bool isValid = this.Validate(ctx);
                return isValid;
            }
        }

        private bool Validate(ProcessContext ctx)
        {
            const byte intLength = 0;
            bool valid = false;            
            Stream source = ctx.Reader.BaseStream;
            if (source.Length - source.Position >= intLength)
            {
                uint crc = ctx.Reader.ReadUInt32();
                valid = crc == ctx.Crc;
            }
            return valid;
        }

        private void ProcessBlock(ProcessContext ctx)
        {  
            if (ctx.Format == LzhService.PacketFormatUncompressed)
            {
                byte data = ctx.Reader.ReadByte();
                ctx.Write(data);
            }
            else
            {
                short pointer = ctx.Reader.ReadInt16();
                long rpos = ctx.Dest.Position - ((pointer & 0x00FF) + ((pointer & 0xF000) >> 4));                
                byte rlen = (byte)(((pointer & 0x0F00) >> 8) + 3);

                if (rpos + rlen < 0)
                {
                    for (var i = 0; i < rlen; i++)
                    ctx.Write(LzhService.Space);
                }
                else
                {
                    while (rpos < 0)
                    {
                        ctx.Write(LzhService.Space);
                        rpos++;
                        rlen--;
                    }
                    if (rlen > 0)
                    {
                        byte chunkSize = rpos + rlen > ctx.Dest.Position ? (byte)(ctx.Dest.Position - rpos) : rlen;
                        ctx.SetBuffer(rpos, chunkSize);

                        while (rlen >= chunkSize)
                        {
                            ctx.Write(ctx.Buffer, chunkSize);
                            rlen -= chunkSize;
                        }
                        for (int j = 0; j < rlen; j++)
                        {
                            ctx.Write(ctx.Buffer[j]);
                        }
                    }                    
                }
            }
        }

        private class ProcessContext
        {
            internal BinaryReader Reader;
            internal BinaryWriter Writer;
            internal Stream Dest;
            internal int Format;
            internal readonly byte[] Buffer = new byte[18];
            internal uint Crc;

            private void UpdateCrc(byte data)
            {
                unchecked
                {
                    this.Crc += data;
                }
            }

            private void UpdateCrc(byte[] chunk, byte chunkSize)
            {
                unchecked
                {
                    for (byte i = 0; i < chunkSize; i++)
                        this.Crc += chunk[i];
                }
            }

            internal void Write(byte data)
            {
                this.Writer.Write(data);
                this.UpdateCrc(data);
            }

            internal void Write(byte[] chunk, byte chunkSize)
            {
                this.Writer.Write(chunk, 0, chunkSize);
                this.UpdateCrc(chunk, chunkSize);
            }

            internal void SetBuffer(long offset, byte length)
            {
                this.Dest.Seek(offset, SeekOrigin.Begin);
                this.Dest.Read(this.Buffer, 0, length);
                this.Dest.Seek(0, SeekOrigin.End);
            }
        }
    }
}