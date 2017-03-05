using System;
using System.IO;
using System.Threading.Tasks;
using NLog;
using PboTools.Domain;
using Util;

namespace PboTools.Service
{
    public class PboPackService : IPboPackService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IPboDiskService pboDiskService;
        private readonly ILzhService lzhService;

        public PboPackService(IPboDiskService pboDiskService, ILzhService lzhService)
        {
            this.pboDiskService = pboDiskService;
            this.lzhService = lzhService;
        }

        public virtual async Task UnpackEntryAsync(PboHeaderEntry entry, Stream pboStream, DirectoryInfo directory, PboUnpackFlags flags)
        {
            Assert.NotNull(entry, nameof(entry));
            Assert.NotNull(pboStream, nameof(pboStream));
            Assert.NotNull(directory, nameof(directory));

            logger.Debug("Unpacking the entry \"{0}\" to the directory \"{1}\" using flags \"{2}\"", entry, directory, flags);

            try
            {
                this.pboDiskService.TryCreateFolder(directory, flags);
                using (Stream stream = this.pboDiskService.CreateFile(entry, directory, flags))
                {
                    if (entry.IsContent)
                    {
                        if (entry.IsCompressed)
                            await this.UnzipFileFromPbo(entry, pboStream, stream).ConfigureAwait(false);
                        else
                            await this.CopyFileFromPboAsync(entry, pboStream, stream).ConfigureAwait(false);

                        await stream.FlushAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (InvalidFilenameException) //illegal characters in path
            {
                logger.Error("Failed to extract entry with an incorrect fileName: \"{0}\"", entry);
            }
        }

        public virtual async Task UnpackPboAsync(PboInfo pboInfo, Stream pboStream, DirectoryInfo directory, PboUnpackFlags flags = PboUnpackFlags.WithFullPath)
        {
            Assert.NotNull(pboInfo, nameof(pboInfo));
            Assert.NotNull(pboStream, nameof(pboStream));
            Assert.NotNull(directory, nameof(directory));

            foreach (PboHeaderEntry fileRecord in pboInfo.FileRecords)
                await this.UnpackEntryAsync(fileRecord, pboStream, directory, flags).ConfigureAwait(false);
        }

        private async Task CopyFileFromPboAsync(PboHeaderEntry entry, Stream pboStream, Stream file)
        {
            logger.Debug("Pbo stream length is \"{0}\", entry data offset is \"{1}\", entry data size is \"{2}\"", pboStream.Length, entry.DataOffset, entry.DataSize);

            var buff = new byte[1024];

            pboStream.Position = entry.DataOffset;
            long dataBlockEnd = entry.DataOffset + entry.DataSize;

            int bytesToRead;

            while ((bytesToRead = (int) Math.Min(dataBlockEnd - pboStream.Position, buff.Length)) > 0)
            {
                int bytesRead = await pboStream.ReadAsync(buff, 0, bytesToRead).ConfigureAwait(false);
                await file.WriteAsync(buff, 0, bytesRead).ConfigureAwait(false);
            }
        }

        private async Task UnzipFileFromPbo(PboHeaderEntry entry, Stream pboStream, Stream file)
        {
            logger.Debug("Pbo stream length is \"{0}\", entry data offset is \"{1}\"", pboStream.Length, entry.DataOffset);
            pboStream.Seek(entry.DataOffset, SeekOrigin.Begin);

            bool success = await this.lzhService.Decompress(pboStream, file, entry.OriginalSize).ConfigureAwait(false);

            if (!success)
                logger.Warn("Could not unpack a compressed entry from the file - CRC error:  \"{0}\"", entry);
        }

        public virtual async Task PackPboAsync(PboInfo pboInfo, Stream pboStream, DirectoryInfo directory)
        {
            Assert.NotNull(pboInfo, nameof(pboInfo));
            Assert.NotNull(pboStream, nameof(pboStream));
            Assert.NotNull(directory, nameof(directory));

            foreach (PboHeaderEntry headerEntry in pboInfo.FileRecords)
                await this.PackEntryAsync(headerEntry, pboStream, directory).ConfigureAwait(false);
        }

        private async Task PackEntryAsync(PboHeaderEntry entry, Stream pboStream, DirectoryInfo directory)
        {
            using (Stream stream = this.pboDiskService.OpenFile(entry, directory))
            {
                await stream.CopyToAsync(pboStream).ConfigureAwait(false);
            }
        }
    }
}