using System.IO;
using System.Text;

namespace PboTools.Service
{
	public class PboBinaryWriter : BinaryWriter
	{
		protected PboBinaryWriter()
		{
		}

		public PboBinaryWriter(Stream output) : base(output)
		{
		}

		public PboBinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
		{
		}

		public PboBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
		{
		}

		public void WriteNullTerminatedString(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			this.BaseStream.Write(bytes, 0, bytes.Length);			
			this.Write((byte)0);
		}
	}
}
