using System;
using System.IO;
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

            TestDelegate caller = () => service.Decompress(null, null, 1);
            var ex1 = Assert.Throws<ArgumentNullException>(caller);
            StringAssert.Contains("source", ex1.Message);

            caller = () => service.Decompress(Stream.Null, null, 1);
            ex1 = Assert.Throws<ArgumentNullException>(caller);
            StringAssert.Contains("dest", ex1.Message);

            caller = () => service.Decompress(Stream.Null, Stream.Null, 0);
            var ex2 = Assert.Throws<ArgumentOutOfRangeException>(caller);
            StringAssert.Contains("targetLength", ex2.Message);
        }

        [Test]
        public void Test_Decompress_Unpacks_Lzh()
        {
            using (Stream originalData = OpenFile(@"TestData\LzhService\gpl-3.0.txt"))
            {
                using (Stream packedData = OpenFile(@"TestData\LzhService\gpl-3.0.lzh"))
                {
                    using (Stream unpackedData = new MemoryStream())
                    {
                        LzhService service = this.GetService();
                        service.Decompress(packedData, unpackedData, originalData.Length);

                        unpackedData.Seek(0, SeekOrigin.Begin);
                        Stream w = File.OpenWrite(@"c:\users\nikita_kobzev\desktop\1.sqf");
                        unpackedData.CopyTo(w);
                        w.Flush();
                        unpackedData.Seek(0, SeekOrigin.Begin);

                        FileAssert.AreEqual(originalData, unpackedData);
                    }
                }
            }
        }        

        private static Stream OpenFile(string fileName)
        {
            string codeBase = typeof(LzhServiceTest).Assembly.CodeBase;
            string folder = Path.GetDirectoryName(codeBase);
            string file = new Uri(Path.Combine(folder, fileName)).AbsolutePath;
            Stream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream;
        }
    }
}