using System;
using System.IO;
using System.Threading.Tasks;
using PboTools.Domain;

namespace PboTools.Service
{
	public class PboPackService : IPboPackService
	{
		private readonly IPboDiskService pboDiskService;
		private readonly ILzhService lzhService;

		public PboPackService(IPboDiskService pboDiskService, ILzhService lzhService)
		{
			this.pboDiskService = pboDiskService;
			this.lzhService = lzhService;
		}

		public async Task UnpackEntryAsync(PboHeaderEntry entry, Stream pboStream, DirectoryInfo directory, PboUnpackFlags flags)
		{
			try
			{
				this.pboDiskService.TryCreateFolder(directory, flags);
				using (Stream stream = this.pboDiskService.CreateFile(entry, directory, flags))
				{
					if (entry.IsCompressed)
						this.UnzipFileFromPbo(entry, pboStream, stream);
					else
						await this.CopyFileFromPboAsync(entry, pboStream, stream).ConfigureAwait(false);

					await stream.FlushAsync().ConfigureAwait(false);
				}
			}
			catch (InvalidFilenameException) //illegal characters in path
			{
				//do nothing				
			}
		}

		public async Task UnpackPboAsync(PboInfo pboInfo, Stream pboStream, DirectoryInfo directory, PboUnpackFlags flags = PboUnpackFlags.WithFullPath)
		{
			foreach (PboHeaderEntry fileRecord in pboInfo.FileRecords)
				await this.UnpackEntryAsync(fileRecord, pboStream, directory, flags).ConfigureAwait(false);
		}

		private async Task CopyFileFromPboAsync(PboHeaderEntry entry, Stream pboStream, Stream file)
		{
			var buff = new byte[1024];

			pboStream.Position = entry.DataOffset;
			long dataBlockEnd = pboStream.Position + entry.DataSize;

			int bytesToRead;

			while ((bytesToRead = (int)Math.Min(dataBlockEnd - pboStream.Position, buff.Length)) > 0)
			{
				var bytesRead = await pboStream.ReadAsync(buff, 0, bytesToRead).ConfigureAwait(false);				
				await file.WriteAsync(buff, 0, bytesRead).ConfigureAwait(false);			
			}
		}

		private void UnzipFileFromPbo(PboHeaderEntry entry, Stream pboStream, Stream file)
		{
			pboStream.Position = entry.DataOffset;
			this.lzhService.Decompress(pboStream, file, entry.OriginalSize);
		}

		public async Task PackPboAsync(PboInfo pboInfo, Stream pboStream, DirectoryInfo directory)
		{
			foreach (PboHeaderEntry headerEntry in pboInfo.FileRecords)
			{
				await this.PackEntryAsync(headerEntry, pboStream, directory).ConfigureAwait(false);
			}
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