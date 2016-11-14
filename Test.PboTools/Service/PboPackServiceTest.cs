using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PboTools.Domain;
using PboTools.Service;
using Util;
using Assert = NUnit.Framework.Assert;

namespace Test.PboTools.Service
{
    public class PboPackServiceTest
    {
        private IPboDiskService diskService;
        private ILzhService lzhService;

        [SetUp]
        public void SetUp()
        {
            this.diskService = Substitute.For<IPboDiskService>();
            this.lzhService = Substitute.For<ILzhService>();
        }

        public PboPackService GetService()
        {
            return new PboPackService(this.diskService, this.lzhService);
        }

        [Test]
        public void Test_UnpackEntryAsync_Throws_If_Called_With_Illegal_Args()
        {
            PboPackService service = this.GetService();

            AsyncTestDelegate caller = async () => await service.UnpackEntryAsync(null, null, null, PboUnpackFlags.None).ConfigureAwait(false);
            var ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("entry", ex.Message);

            caller = async () => await service.UnpackEntryAsync(new PboHeaderEntry(), null, null, PboUnpackFlags.None).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("pboStream", ex.Message);

            caller = async () => await service.UnpackEntryAsync(new PboHeaderEntry(), Stream.Null, null, PboUnpackFlags.None).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("directory", ex.Message);
        }

        [Test]
        public async Task Test_UnpackEntryAsync_Unpacks_Uncompressed_Entry_To_Stream()
        {
            //prepare input data
            var pboFlags = PboUnpackFlags.CreateFolder | PboUnpackFlags.OverwriteFiles;
            var dir = new DirectoryInfo(Path.GetTempPath());
            var entry = new PboHeaderEntry
            {
                FileName = "mission.sqm",
                DataOffset = 10,
                DataSize = 20,
                OriginalSize = 20,
                PackingMethod = PboPackingMethod.Uncompressed
            };
            var buf = new byte[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50
            };
            var pboStream = new MemoryStream(buf);

            //internal services mocks
            var outputBytes = new byte[entry.OriginalSize + 5];
            var outputStream = new MemoryStream(outputBytes);
            this.diskService.CreateFile(null, null, PboUnpackFlags.None).ReturnsForAnyArgs(outputStream);

            //call the method
            PboPackService service = this.GetService();
            await service.UnpackEntryAsync(entry, pboStream, dir, pboFlags).ConfigureAwait(false);

            //verify the mocks have been called
            this.diskService.Received(1).TryCreateFolder(dir, pboFlags);
            this.diskService.Received(1).CreateFile(entry, dir, pboFlags);
            this.lzhService.DidNotReceiveWithAnyArgs().Decompress(null, null, 0).IgnoreAwait();

            //check output results
            Assert.False(outputStream.CanRead);
            byte[] expectedContents = new byte[]
            {
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                0, 0, 0, 0, 0
            };
            CollectionAssert.AreEqual(expectedContents, outputBytes);
        }

        [Test]
        public async Task Test_UnpackEntryAsync_Unpacks_Compressed_Entry_To_Stream()
        {
            //prepare input data
            var pboFlags = PboUnpackFlags.CreateFolder | PboUnpackFlags.OverwriteFiles;
            var dir = new DirectoryInfo(Path.GetTempPath());
            var entry = new PboHeaderEntry
            {
                FileName = "mission.sqm",
                DataOffset = 10,
                DataSize = 50,
                OriginalSize = 20,
                PackingMethod = PboPackingMethod.Packed
            };
            var buf = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
            var pboStream = new MemoryStream(buf);

            //internal services mocks
            var outputStream = new MemoryStream();
            this.diskService.CreateFile(null, null, PboUnpackFlags.None).ReturnsForAnyArgs(outputStream);

            //call the method
            PboPackService service = this.GetService();
            await service.UnpackEntryAsync(entry, pboStream, dir, pboFlags).ConfigureAwait(false);

            //verify the mocks have been called
            this.diskService.Received(1).TryCreateFolder(dir, pboFlags);
            this.diskService.Received(1).CreateFile(entry, dir, pboFlags);
            this.lzhService.Received(1).Decompress(pboStream, outputStream, entry.OriginalSize).IgnoreAwait();

            //verify the pboStream position has been set for unpacking
            Assert.AreEqual(entry.DataOffset, pboStream.Position);
        }

        [Test]
        public void Test_UnpackEntryAsync_Handles_Incorrect_File_Names()
        {
            //prepare input data
            var dir = new DirectoryInfo(Path.GetTempPath());
            var entry = new PboHeaderEntry
            {
                FileName = "mission.sqm",
                DataOffset = 10,
                DataSize = 20,
                OriginalSize = 20,
                PackingMethod = PboPackingMethod.Uncompressed
            };
            var pboStream = new MemoryStream();

            //internal services mocks
            this.diskService.CreateFile(null, null, PboUnpackFlags.None).ThrowsForAnyArgs(p => new InvalidFilenameException(entry.FileName, null));

            //call the method
            PboPackService service = this.GetService();
            AsyncTestDelegate caller = async () => await service.UnpackEntryAsync(entry, pboStream, dir, PboUnpackFlags.None).ConfigureAwait(false);
            Assert.DoesNotThrowAsync(caller);
        }


        [Test]
        public void Test_UnpackPboAsync_Throws_If_Called_With_Illegal_Args()
        {
            PboPackService service = this.GetService();

            AsyncTestDelegate caller = async () => await service.UnpackPboAsync(null, null, null, PboUnpackFlags.None).ConfigureAwait(false);
            var ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("pboInfo", ex.Message);

            caller = async () => await service.UnpackPboAsync(new PboInfo(), null, null, PboUnpackFlags.None).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("pboStream", ex.Message);

            caller = async () => await service.UnpackPboAsync(new PboInfo(), Stream.Null, null, PboUnpackFlags.None).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("directory", ex.Message);
        }

        [Test]
        public async Task Test_UnpackPboAsync_Unpacks_All_Pbo_Entries()
        {
            //prepare input data
            var pboFlags = PboUnpackFlags.CreateFolder | PboUnpackFlags.OverwriteFiles;
            var dir = new DirectoryInfo(Path.GetTempPath());
            var pboInfo = new PboInfo
            {
                FileRecords = new List<PboHeaderEntry>
                {
                    new PboHeaderEntry {FileName = "file1.txt"},
                    new PboHeaderEntry {FileName = "file2.txt"}
                }
            };

            var pboStream = new MemoryStream();

            //internal services mocks
            PboPackService service = Substitute.ForPartsOf<PboPackService>(this.diskService, this.lzhService);
            service.WhenForAnyArgs(p => p.UnpackEntryAsync(null, null, null, PboUnpackFlags.None).IgnoreAwait()).DoNotCallBase();

            //call the method
            await service.UnpackPboAsync(pboInfo, pboStream, dir, pboFlags).ConfigureAwait(false);

            //verify the mocks have been called
            service.Received(1).UnpackEntryAsync(pboInfo.FileRecords[0], pboStream, dir, pboFlags).IgnoreAwait();
            service.Received(1).UnpackEntryAsync(pboInfo.FileRecords[1], pboStream, dir, pboFlags).IgnoreAwait();
        }


        [Test]
        public void Test_PackPboAsync_Throws_If_Called_With_Illegal_Args()
        {
            PboPackService service = this.GetService();

            AsyncTestDelegate caller = async () => await service.PackPboAsync(null, null, null).ConfigureAwait(false);
            var ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("pboInfo", ex.Message);

            caller = async () => await service.PackPboAsync(new PboInfo(), null, null).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("pboStream", ex.Message);

            caller = async () => await service.PackPboAsync(new PboInfo(), Stream.Null, null).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caller);
            StringAssert.Contains("directory", ex.Message);
        }

        [Test]
        public async Task Test_PackPboAsync_Packs_PboInfo_Into_Stream()
        {
            //prepare input data
            var dir = new DirectoryInfo(Path.GetTempPath());
            var pboInfo = new PboInfo
            {
                FileRecords = new List<PboHeaderEntry>
                {
                    new PboHeaderEntry {FileName = "file1.txt"},
                    new PboHeaderEntry {FileName = "file2.txt"}
                }
            };

            //internal services mocks
            var data1 = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var data2 = new byte[] {11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
            this.diskService.OpenFile(pboInfo.FileRecords[0], dir).Returns(new MemoryStream(data1));
            this.diskService.OpenFile(pboInfo.FileRecords[1], dir).Returns(new MemoryStream(data2));

            var pboStream = new MemoryStream();

            //call the method
            PboPackService service = this.GetService();
            await service.PackPboAsync(pboInfo, pboStream, dir).ConfigureAwait(false);

            //verify the mocks have been called
            this.diskService.Received(1).OpenFile(pboInfo.FileRecords[0], dir);
            this.diskService.Received(1).OpenFile(pboInfo.FileRecords[1], dir);

            //verify the pboStream has been inflated
            Assert.AreEqual(data1.Length + data2.Length, pboStream.Length);
            Assert.True(pboStream.CanRead);
            IEnumerable<byte> expectedData = data1.Concat(data2);
            CollectionAssert.AreEqual(expectedData, pboStream.ToArray());
        }
    }
}