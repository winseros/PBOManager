using System.IO;
using System.Threading.Tasks;

namespace PboTools.Service
{
	public interface ILzhService
	{
		Task Decompress(Stream source, Stream dest, long targetLength);
	}
}
