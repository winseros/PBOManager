using System.IO;
using System.Threading.Tasks;
using PboTools.Domain;
using Util;

namespace PboTools.Service
{
    public class PboArchiverService : IPboArchiverService
    {
        private readonly IPboPackService pboPackService;

        public PboArchiverService(IPboPackService pboPackService)
        {
            this.pboPackService = pboPackService;
        }

        public async Task PackDirecoryAsync(string directoryPath, string pboPath)
        {
            Assert.NotNull(directoryPath, nameof(directoryPath));
            Assert.NotNull(pboPath, nameof(pboPath));

            var directory = new DirectoryInfo(directoryPath);
            var infoService = new PboInfoService();
            PboInfo info = infoService.CollectPboInfo(directory);

            using (Stream stream = File.Open(pboPath, FileMode.Create, FileAccess.Write))
            using (var writer = new PboBinaryWriter(stream))
            {
                infoService.WritePboInfo(writer, info);
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
                var service = new PboInfoService();
                PboInfo info = service.ReadPboInfo(reader);
                await this.pboPackService.UnpackPboAsync(info, str, new DirectoryInfo(directoryPath)).ConfigureAwait(false);
            }
        }
    }
}