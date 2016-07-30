using System.IO;
using System.Text;

namespace PboTools.Service
{
	public class PboBinaryReader : BinaryReader
	{
		public PboBinaryReader(Stream input) : base(input)
		{
		}

		public PboBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
		{
		}

		public PboBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
		{
		}

		public string ReadNullTerminatedString()
		{
			var builder = new StringBuilder();
			
			byte b;
			while ((b = this.ReadByte()) != 0)
				builder.Append((char)b);
			var result = builder.ToString();

			return result;
		}
	}
}
