using System;

namespace PboTools.Service
{
	public class FileAlreadyExistsException : ApplicationException
	{
		public FileAlreadyExistsException(string fileName)
		{
			this.FileName = fileName;
		}

		public string FileName { get; }
	}
}
