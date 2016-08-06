using System;
using System.IO;
using NUnit.Framework;
using PboTools.Domain;
using PboTools.Service;

namespace Test.PboTools.Service
{
    public class PboDiskServiceTest
    {
        private PboDiskService GetService()
        {
            return new PboDiskService();
        }

        [Test]
        public void Test_TryCreateFolder_Throws_If_Called_With_Illegal_Args()
        {
            PboDiskService service = this.GetService();

            TestDelegate caller = () => service.TryCreateFolder(null, 0);
            var ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("folder", ex.Message);
        }

        [Test]
        public void Test_TryCreateFolder_Creates_The_Folder_If_Flags_Were_Set()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var dir = new DirectoryInfo(path);

            PboDiskService service = this.GetService();
            service.TryCreateFolder(dir, PboUnpackFlags.CreateFolder);

            bool exists = Directory.Exists(path);
            Assert.True(exists);
        }

        [Test]
        public void Test_TryCreateFolder_Throws_If_Flags_Were_Not_Set()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var dir = new DirectoryInfo(path);

            PboDiskService service = this.GetService();
            TestDelegate caller = () => service.TryCreateFolder(dir, PboUnpackFlags.None);

            var ex = Assert.Throws<DirectoryNotFoundException>(caller);
            Assert.AreEqual(path, ex.Message);
        }


        [Test]
        public void Test_CreateFile_Throws_If_Called_With_Illegal_Args()
        {
            PboDiskService service = this.GetService();

            TestDelegate caller = () => service.CreateFile(null, null, 0);
            var ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("entry", ex.Message);

            caller = () => service.CreateFile(new PboHeaderEntry(), null, 0);
            ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("folder", ex.Message);

            caller = () => service.CreateFile(new PboHeaderEntry(), new DirectoryInfo(@"C:\"), 0);
            ex = Assert.Catch<ArgumentException>(caller);
            Assert.AreEqual("The entry provided should have a non-empty FileName", ex.Message);
        }

        [Test]
        public void Test_CreateFile_Throws_If_File_Exists_And_Overwrite_Flag_Not_Set()
        {
            string fileName = "existing-file.txt";
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);

            string fileFullName = Path.Combine(path, fileName);
            File.WriteAllText(fileFullName, "no-text");

            PboDiskService service = this.GetService();
            TestDelegate caller = () => service.CreateFile(new PboHeaderEntry {FileName = fileName}, new DirectoryInfo(path), 0);
            var ex = Assert.Throws<FileAlreadyExistsException>(caller);
            Assert.AreEqual(fileFullName, ex.FileName);
        }

        [Test]
        public void Test_CreateFile_Overwrites_The_Existing_File_If_Overwrite_Flag_Is_Set()
        {
            string fileName = "existing-file.txt";
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);

            string fileFullName = Path.Combine(path, fileName);
            File.WriteAllText(fileFullName, "no-text");

            PboDiskService service = this.GetService();
            Stream stream = service.CreateFile(new PboHeaderEntry {FileName = fileName}, new DirectoryInfo(path), PboUnpackFlags.OverwriteFiles);

            Assert.IsNotNull(stream);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(0, stream.Length);
        }

        [Test]
        public void Test_CreateFile_Creates_A_New_File()
        {
            string fileName = "existing-file.txt";
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            PboDiskService service = this.GetService();
            Stream stream = service.CreateFile(new PboHeaderEntry {FileName = fileName}, new DirectoryInfo(path), 0);

            Assert.IsNotNull(stream);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(0, stream.Length);

            string fileFullName = Path.Combine(path, fileName);
            Assert.AreEqual(((FileStream) stream).Name, fileFullName);
        }

        [Test]
        public void Test_CreateFile_Throws_If_FileName_Contains_Illegal_Chars()
        {
            PboDiskService service = this.GetService();
            TestDelegate caller = () => service.CreateFile(new PboHeaderEntry {FileName = "*.*"}, new DirectoryInfo(@"C:\"), 0);
            var ex = Assert.Throws<InvalidFilenameException>(caller);
            Assert.AreEqual("*.*", ex.FileName);
        }


        [Test]
        public void Test_OpenFile_Throws_If_Called_With_Illegal_Args()
        {
            PboDiskService service = this.GetService();

            TestDelegate caller = () => service.OpenFile(null, null);
            var ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("entry", ex.Message);

            caller = () => service.OpenFile(new PboHeaderEntry(), null);
            ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("folder", ex.Message);
        }

        [Test]
        public void Test_OpenFile_Returns_An_Opened_Stream()
        {
            string fileName = "existing-file.txt";
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);

            string fileFullName = Path.Combine(path, fileName);
            File.WriteAllText(fileFullName, "no-text");

            PboDiskService service = this.GetService();
            Stream stream = service.OpenFile(new PboHeaderEntry {FileName = fileName}, new DirectoryInfo(path));

            Assert.IsNotNull(stream);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(7, stream.Length);

            Assert.AreEqual(((FileStream)stream).Name, fileFullName);
        }
    }
}