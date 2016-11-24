using PboTools.Domain;
using ReactiveUI;
using Util;

namespace PboManager.Components.PboTree
{
    public class PboTreeModel : ReactiveObject
    {
        private PboNodeModel rootNode;

        public void LoadPbo(PboInfo pboInfo, string fileName)
        {
            Assert.NotNull(pboInfo, nameof(pboInfo));
            Assert.NotNull(fileName, nameof(fileName));

            var builder = new PboNodeBuilder(fileName);
            foreach (PboHeaderEntry headerEntry in pboInfo.FileRecords)
                builder.AddEntry(headerEntry);

            this.rootNode = builder.Build();

            using (this.Root.SuppressChangeNotifications())
            {
                this.Root.Clear();
                this.Root.Add(this.rootNode);
            }
           
        }

        public ReactiveList<PboNodeModel> Root { get; } = new ReactiveList<PboNodeModel>();
    }
}
