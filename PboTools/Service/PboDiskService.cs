using System;
using System.IO;
using PboTools.Domain;
using Util;

namespace PboTools.Service
{
	public class PboDiskService : IPboDiskService
	{
		public void TryCreateFolder(DirectoryInfo folder, PboUnpackFlags flags)
		{
            Assert.NotNull(folder, nameof(folder));

			var canCreateFolders = (flags & PboUnpackFlags.CreateFolder) == PboUnpackFlags.CreateFolder;
			if (canCreateFolders)
				folder.Create();
			else
				throw new DirectoryNotFoundException(folder.FullName);
		}

		public Stream CreateFile(PboHeaderEntry entry, DirectoryInfo folder, PboUnpackFlags flags)
		{
            Assert.NotNull(entry, nameof(entry));
            Assert.NotNull(folder, nameof(folder));

            if (string.IsNullOrEmpty(entry.FileName))
				throw new ArgumentException("The entry provided should have a non-empty FileName");

			var useFullPath = (flags & PboUnpackFlags.WithFullPath) == PboUnpackFlags.WithFullPath;
			string fileName = useFullPath ? entry.FileName : Path.GetFileName(entry.FileName);
			string filePath = Path.Combine(folder.FullName, fileName);

			string fileFolder = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(fileFolder))
				Directory.CreateDirectory(fileFolder);

			if (File.Exists(filePath))
			{
				var overwrite = (flags & PboUnpackFlags.OverwriteFiles) == PboUnpackFlags.OverwriteFiles;
				if (!overwrite)
					throw new FileAlreadyExistsException(filePath);
			}

			try
			{
				Stream stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
				return stream;
			}
			catch (ArgumentException ex)
			{
				throw new InvalidFilenameException(entry.FileName, ex);
			}
		}

		public Stream OpenFile(PboHeaderEntry entry, DirectoryInfo folder)
		{
            Assert.NotNull(entry, nameof(entry));
            Assert.NotNull(folder, nameof(folder));

            string filePath = Path.Combine(folder.FullName, entry.FileName);
			Stream result = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			return result;
		}
	}

	public class InvalidFilenameException : ApplicationException
	{
		public InvalidFilenameException(string fileName, ArgumentException innerException)
			: base(fileName, innerException)
		{			
		}
	}
}