using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using PboTools.Domain;
using PboTools.Service;
using Assert = NUnit.Framework.Assert;
using Util;

namespace Test.PboTools.Service
{
    public class PboArchiverServiceTest
    {
        private IPboInfoService pboInfoService;
        private IPboPackService pboPackService;

        [SetUp]
        public void SetUp()
        {
            this.pboInfoService = Substitute.For<IPboInfoService>();
            this.pboPackService = Substitute.For<IPboPackService>();
        }

        private PboArchiverService GetService()
        {
            return new PboArchiverService(this.pboInfoService, this.pboPackService);
        }

        [Test]
        public void Test_PackDirecoryAsync_Throws_If_Called_With_Illegal_Args()
        {
            PboArchiverService service = this.GetService();

            AsyncTestDelegate caler = async () => await service.PackDirecoryAsync(null, null).ConfigureAwait(false);
            var ex = Assert.CatchAsync<ArgumentException>(caler);
            StringAssert.Contains("directoryPath", ex.Message);

            caler = async () => await service.PackDirecoryAsync("some-path", null).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caler);
            StringAssert.Contains("pboPath", ex.Message);
        }

        [Test]
        public async Task Test_PackDirecoryAsync_Packs_Directory()
        {
            //prepare input data
            string dirName = Path.GetTempPath();
            string pboName = Path.Combine(dirName, "temp.pbo");
            File.Delete(pboName);

            //internal services mocks
            var pboInfo = new PboInfo();
            this.pboInfoService.CollectPboInfo(null).ReturnsForAnyArgs(pboInfo);
            this.pboInfoService.WhenForAnyArgs(infoService => infoService.CollectPboInfo(null))
                .Do(callInfo =>
                {
                    var dir = callInfo.Arg<DirectoryInfo>();
                    Assert.AreEqual(dirName, dir.FullName);                    
                });

            this.pboPackService.WhenForAnyArgs(packService => packService.PackPboAsync(null, null, null))
                .Do(callInfo =>
                {
                    var dir = callInfo.Arg<DirectoryInfo>();
                    Assert.AreEqual(dirName, dir.FullName);
                });

            //call the method
            PboArchiverService service = this.GetService();
            await service.PackDirecoryAsync(dirName, pboName).ConfigureAwait(false);

            //verify the mocks have been called
            this.pboInfoService.Received(1).CollectPboInfo(Arg.Any<DirectoryInfo>());
            this.pboInfoService.Received(1).WritePboInfo(Arg.Any<PboBinaryWriter>(), pboInfo);
            this.pboPackService.Received(1).PackPboAsync(pboInfo, Arg.Any<Stream>(), Arg.Any<DirectoryInfo>()).IgnoreAwait();

            //verify the file has been created
            Assert.True(File.Exists(pboName));
        }


        [Test]
        public void Test_UnpackPboAsync_Throws_If_Called_With_Illegal_Args()
        {
            PboArchiverService service = this.GetService();

            AsyncTestDelegate caler = async () => await service.UnpackPboAsync(null, null).ConfigureAwait(false);
            var ex = Assert.CatchAsync<ArgumentException>(caler);
            StringAssert.Contains("pboPath", ex.Message);

            caler = async () => await service.UnpackPboAsync("some-path", null).ConfigureAwait(false);
            ex = Assert.CatchAsync<ArgumentException>(caler);
            StringAssert.Contains("directoryPath", ex.Message);
        }

        [Test]
        public async Task Test_UnpackPboAsync_Unpacks_File()
        {
            //prepare input data
            string dirName = Path.GetTempPath();
            string pboName = Path.Combine(dirName, "temp.pbo");

            using (File.Open(pboName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //just create an empty file                
            }

            //internal services mocks
            var pboInfo = new PboInfo();
            this.pboInfoService.ReadPboInfo(null).ReturnsForAnyArgs(pboInfo);
            this.pboPackService.WhenForAnyArgs(packService => packService.UnpackPboAsync(null, null, null))
                .Do(callInfo =>
                {
                    var dir = callInfo.Arg<DirectoryInfo>();
                    Assert.AreEqual(dirName, dir.FullName);
                });

            //call the method
            PboArchiverService service = this.GetService();
            await service.UnpackPboAsync(pboName, dirName).ConfigureAwait(false);

            //verify the mocks have been called
            this.pboInfoService.Received(1).ReadPboInfo(Arg.Any<PboBinaryReader>());
            this.pboPackService.Received(1).UnpackPboAsync(pboInfo, Arg.Any<Stream>(), Arg.Any<DirectoryInfo>()).IgnoreAwait();
        }
    }
}
