using System.IO;

namespace PboTools.Service
{
	public interface ILzhService
	{
		void Decompress(Stream source, Stream dest, long targetLength);
	}
}
