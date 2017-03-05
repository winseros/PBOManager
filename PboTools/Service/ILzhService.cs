using System.IO;
using System.Threading.Tasks;

namespace PboTools.Service
{
	public interface ILzhService
	{
		Task<bool> Decompress(Stream source, Stream dest, long targetLength);
	}
}
