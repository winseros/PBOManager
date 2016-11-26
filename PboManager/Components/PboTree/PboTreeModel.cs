using System.IO;
using PboTools.Domain;
using ReactiveUI;
using Util;

namespace PboManager.Components.PboTree
{
    public class PboTreeModel : ReactiveObject
    {
        private readonly IPboTreeContext context;
        private PboTreeNode rootNode;

        public PboTreeModel(IPboTreeContext context)
        {
            this.context = context;
        }

        public void LoadPbo(PboInfo pboInfo, string fileName)
        {
            Assert.NotNull(pboInfo, nameof(pboInfo));
            Assert.NotNull(pboInfo.FileRecords, "pboInfo.FileRecords");
            Assert.NotNull(fileName, nameof(fileName));

            this.rootNode = new PboTreeDirectory(this.context)
            {
                Name = Path.GetFileName(fileName)
            };
            foreach (PboHeaderEntry fileRecord in pboInfo.FileRecords)
                this.rootNode.AddEntry(fileRecord);

            this.rootNode.Sort();

            using (this.rootNode.SuppressChangeNotifications())
            {
                this.Root.Clear();
                this.Root.Add(this.rootNode);
            }
        }

        public ReactiveList<PboTreeNode> Root { get; } = new ReactiveList<PboTreeNode>();
    }
}
