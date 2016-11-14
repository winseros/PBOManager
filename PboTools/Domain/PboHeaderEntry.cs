using System.IO;
using System.Text;
using Util;

namespace PboTools.Domain
{
    public class PboHeaderEntry
    {
        public static readonly int BaseEntrySize = 21; //entry has 5 x 4bytes fields + 1 zero-byte of entry name

        public static int SizeOf(PboHeaderEntry entry)
        {
            Assert.NotNull(entry, nameof(entry));
            int nameLength = string.IsNullOrEmpty(entry.FileName) ? 0 : entry.FileName.Length;
            return nameLength + PboHeaderEntry.BaseEntrySize;
        }

        public static PboHeaderEntry CreateBoundary()
        {
            return new PboHeaderEntry
            {
                FileName = string.Empty,
                PackingMethod = PboPackingMethod.Uncompressed
            };
        }

        public static PboHeaderEntry CreateSignature()
        {
            PboHeaderEntry result = CreateBoundary();
            result.PackingMethod = PboPackingMethod.Product;
            return result;
        }

        public string FileName { get; set; }

        public PboPackingMethod PackingMethod { get; set; }

        public int OriginalSize { get; set; }

        public int Reserved { get; set; }

        public int TimeStamp { get; set; }

        public int DataSize { get; set; }

        public long DataOffset { get; set; }

        public bool IsBoundary
        {
            get { return string.IsNullOrEmpty(this.FileName); }
        }

        public bool IsSignature
        {
            get { return this.PackingMethod == PboPackingMethod.Product; }
        }

        public bool IsCompressed
        {
            get { return this.PackingMethod == PboPackingMethod.Packed && this.OriginalSize != this.DataSize; }
        }

        public string GetShortFileName()
        {
            return Path.GetFileName(this.FileName);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("FileName:\"")
                .Append(this.FileName)
                .Append("\";PackingMethod:")
                .Append(this.PackingMethod)
                .Append(";OriginalSize:")
                .Append(this.OriginalSize)
                .Append(";Timestamp:")
                .Append(this.TimeStamp)
                .Append(";DataSize:")
                .Append(this.DataSize)
                .Append(";DataOffset:")
                .Append(this.DataOffset);

            return builder.ToString();
        }
    }
}