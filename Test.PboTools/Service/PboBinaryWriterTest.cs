using System.IO;
using NUnit.Framework;
using PboTools.Service;

namespace Test.PboTools.Service
{
    public class PboBinaryWriterTest
    {
        private PboBinaryWriter GetWriter(Stream stream)
        {
            return new PboBinaryWriter(stream);
        }

        [Test]
        public void Test_WriteNullTerminatedString_Writes_The_String()
        {
            var buffer = new byte[] {0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01 };
            using (var stream = new MemoryStream(buffer))
            {
                PboBinaryWriter writer = this.GetWriter(stream);
                writer.WriteNullTerminatedString("qwerty");
                writer.Flush();

                var expected = new byte[] { 0x71, 0x77, 0x65, 0x72, 0x74, 0x79, 0x00, 0x01, 0x01, 0x01 };
                CollectionAssert.AreEqual(expected, buffer);
            }
        }
    }
}
