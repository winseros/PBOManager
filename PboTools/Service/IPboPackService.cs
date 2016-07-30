using System.IO;
using System.Threading.Tasks;
using PboTools.Domain;

namespace PboTools.Service
{
	public interface IPboPackService
	{
		Task UnpackEntryAsync(PboHeaderEntry entry, Stream pboStream, DirectoryInfo directory, PboUnpackFlags flags = PboUnpackFlags.None);

		Task UnpackPboAsync(PboInfo pboInfo, Stream pboStream, DirectoryInfo directory, PboUnpackFlags flags = PboUnpackFlags.WithFullPath);

		Task PackPboAsync(PboInfo pboInfo, Stream pboStream, DirectoryInfo directory);
	}
}
