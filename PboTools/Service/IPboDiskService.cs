using System.IO;
using PboTools.Domain;

namespace PboTools.Service
{
	public interface IPboDiskService
	{
		void TryCreateFolder(DirectoryInfo folder, PboUnpackFlags flags);

		Stream CreateFile(PboHeaderEntry entry, DirectoryInfo folder, PboUnpackFlags flags);

		Stream OpenFile(PboHeaderEntry entry, DirectoryInfo folder);
	}
}
