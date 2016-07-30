using System.Collections.Generic;
using System.Collections.Specialized;

namespace PboTools.Domain
{
	public class PboInfo
	{
		public PboHeaderEntry Signature { get; set; }

		public NameValueCollection HeaderExtensions { get; set; }

		public IList<PboHeaderEntry> FileRecords { get; set; }

		public long DataBlockStart { get; set; }

		public byte[] Checksum { get; set; }

		public PboHeaderNode GetRecordsAsTree()
		{
			var root = new PboHeaderNode();
			foreach (PboHeaderEntry headerEntry in this.FileRecords)
			{
				root.AddEntry(headerEntry);
			}
			return root;
		}
	}
}
