using System.IO;
using System.Windows.Media;
using PboManager.Services.FileIconService;
using PboTools.Domain;
using ReactiveUI;

namespace PboManager.Components.PboTree
{
    public class PboNodeModel : ReactiveObject
    {
        private readonly PboHeaderEntry entry;
        private readonly IPboTreeContext context;
        private string nodeName;

        public PboNodeModel(PboHeaderEntry entry, IPboTreeContext context)
        {
            this.entry = entry;
            this.context = context;
        }

        public void Initialize()
        {
            IFileIconService service = this.context.GetFileIconService();
            this.Icon = this.IsDirectory ? service.GetDirectoryIcon() : service.GetFileIcon(Path.GetExtension(this.entry.FileName));
        }

        public string NodeName
        {
            get { return this.nodeName; }
            set { this.RaiseAndSetIfChanged(ref this.nodeName, value); }
        }

        public PboNodeModel Parent { get; set; }

        public ImageSource Icon { get; private set; }

        public bool IsDirectory => this.entry == null;

        public IReactiveList<PboNodeModel> Children { get; } = new ReactiveList<PboNodeModel>();
    }
}