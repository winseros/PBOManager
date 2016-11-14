using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using PboTools.Service;

namespace Test.PboTools.Service
{
    [TestFixture]
    public class LzhServiceTest
    {
        private LzhService GetService()
        {
            return new LzhService();
        }

        [Test]
        public void Test_Decompress_Throws_If_Called_With_Illegal_Args()
        {
            LzhService service = this.GetService();

            AsyncTestDelegate caller = async () => await service.Decompress(null, null, 1).ConfigureAwait(false);
            var ex1 = Assert.ThrowsAsync<ArgumentNullException>(caller);
            StringAssert.Contains("source", ex1.Message);

            caller = async () => await service.Decompress(Stream.Null, null, 1).ConfigureAwait(false);
            ex1 = Assert.ThrowsAsync<ArgumentNullException>(caller);
            StringAssert.Contains("dest", ex1.Message);

            caller = async () => await service.Decompress(Stream.Null, Stream.Null, 0).ConfigureAwait(false);
            var ex2 = Assert.ThrowsAsync<ArgumentOutOfRangeException>(caller);
            StringAssert.Contains("targetLength", ex2.Message);
        }

        [TestCase("gpl-3.0.txt", "gpl-3.0.lzh")]
        public async Task Test_Decompress_Unpacks_Lzh(string original, string source)
        {
            using (Stream originalData = OpenFile(original))
            using (Stream packedData = OpenFile(source))
            using (Stream unpackedData = new MemoryStream())
            {
                LzhService service = this.GetService();
                await service.Decompress(packedData, unpackedData, originalData.Length).ConfigureAwait(false);

                FileAssert.AreEqual(originalData, unpackedData);
            }
        }

        private static Stream OpenFile(string fileName)
        {
            string codeBase = typeof(LzhServiceTest).Assembly.CodeBase;
            string folder = Path.Combine(Path.GetDirectoryName(codeBase), @"TestData\LzhService\");
            string file = new Uri(Path.Combine(folder, fileName)).AbsolutePath;
            Stream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream;
        }
    }
}