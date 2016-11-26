using System;
using System.Reactive.Linq;
using PboManager.Services.FileIconService;
using PboTools.Domain;
using ReactiveUI;

namespace PboManager.Components.PboTree
{
    public class PboTreeFile : PboTreeNode
    {
        public PboTreeFile(IPboTreeContext context)
        {
            IFileIconService service = context.GetFileIconService();
            this.WhenAnyValue(node => node.Name)
                .Where(name => !string.IsNullOrEmpty(name))
                .Select(name => service.GetFileIcon(name))
                .BindTo(this, file => file.Icon);                
        }

        public PboPackingMethod PackingMethod { get; set; }

        public int OriginalSize { get; set; }

        public int Reserved { get; set; }

        public int TimeStamp { get; set; }

        public int DataSize { get; set; }

        public long DataOffset { get; set; }

        public override void AddEntry(PboHeaderEntry entry)
        {
            throw new InvalidOperationException("Can not add an entry to a file node");
        }

        internal override PboTreeNode GeOrCreateChildDirectory(string name)
        {
            throw new InvalidOperationException("Can not get a child directory of a file node");
        }

        internal override void AddChildFile(string name, PboHeaderEntry entry)
        {
            throw new InvalidOperationException("Can not add a child file to a file node");
        }
    }
}
