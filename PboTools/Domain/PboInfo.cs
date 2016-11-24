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
    }
}