using System.IO;
using System.Threading.Tasks;
using PboTools.Domain;
using Util;

namespace PboTools.Service
{
    public class PboArchiverService : IPboArchiverService
    {
        private readonly IPboInfoService pboInfoService;
        private readonly IPboPackService pboPackService;

        public PboArchiverService(IPboInfoService pboInfoService, IPboPackService pboPackService)
        {
            this.pboInfoService = pboInfoService;
            this.pboPackService = pboPackService;
        }

        public async Task PackDirecoryAsync(string directoryPath, string pboPath)
        {
            Assert.NotNull(directoryPath, nameof(directoryPath));
            Assert.NotNull(pboPath, nameof(pboPath));

            var directory = new DirectoryInfo(directoryPath);
            PboInfo info = this.pboInfoService.CollectPboInfo(directory);

            using (Stream stream = File.Open(pboPath, FileMode.Create, FileAccess.Write))
            using (var writer = new PboBinaryWriter(stream))
            {
                this.pboInfoService.WritePboInfo(writer, info);
                await this.pboPackService.PackPboAsync(info, stream, directory).ConfigureAwait(false);
            }
        }

        public async Task UnpackPboAsync(string pboPath, string directoryPath)
        {
            Assert.NotNull(pboPath, nameof(pboPath));
            Assert.NotNull(directoryPath, nameof(directoryPath));

            using (Stream str = File.Open(pboPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new PboBinaryReader(str))
            {
                PboInfo info = this.pboInfoService.ReadPboInfo(reader);
                await this.pboPackService.UnpackPboAsync(info, str, new DirectoryInfo(directoryPath)).ConfigureAwait(false);
            }
        }

        public PboInfo GetPboInfo(string pboPath)
        {
            Assert.NotNull(pboPath, nameof(pboPath));

            using (Stream str = File.Open(pboPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new PboBinaryReader(str))
            {
                PboInfo info = this.pboInfoService.ReadPboInfo(reader);
                return info;
            }
        }
    }
}