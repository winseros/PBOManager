using System.IO;
using NUnit.Framework;
using PboTools.Service;

namespace Test.PboTools.Service
{
    public class PboBinaryReaderTest
    {
        public PboBinaryReader GetReader(Stream stream)
        {
            return new PboBinaryReader(stream);
        }

        [Test]
        public void Test_ReadNullTerminatedString_Returns_The_String()
        {
            var bytes = new byte[] {0x71, 0x77, 0x65, 0x72, 0x74, 0x79, 0x00, 0x75, 0x69, 0x6f, 0x70};//qwerty0uiop
            using (var stream = new MemoryStream(bytes))
            {
                PboBinaryReader reader = this.GetReader(stream);
                string str = reader.ReadNullTerminatedString();
                Assert.AreEqual("qwerty", str);
            }
        }
    }
}