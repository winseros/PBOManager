using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using PboManager.Components.TreeMenu;
using PboManager.Services.FileIconService;
using PboTools.Domain;

namespace PboManager.Components.PboTree
{
    public class PboNodeModel : ViewModel
    {
        private readonly IPboTreeContext context;

        private string name;
        private readonly Lazy<ImageSource> icon;
        private readonly Lazy<TreeMenuModel> menu;

        public PboNodeModel(PboNodeModelContext nodeModelContext, IPboTreeContext context)
        {
            this.context = context;
            this.name = nodeModelContext.Name;
            this.Parent = nodeModelContext.Parent ?? this;
            this.icon = new Lazy<ImageSource>(this.GetIcon, LazyThreadSafetyMode.None);
            this.menu = new Lazy<TreeMenuModel>(this.GetMenu, LazyThreadSafetyMode.None);
        }

        private ImageSource GetIcon()
        {
            IFileIconService fileIconService = this.context.GetFileIconService();
            bool isFolder = this.Children.Count > 0;
            ImageSource result = isFolder
                ? fileIconService.GetDirectoryIcon()
                : fileIconService.GetFileIcon(Path.GetExtension(this.name));

            return result;
        }

        private TreeMenuModel GetMenu()
        {
            TreeMenuModel treeMenu = this.context.GetTreeMenu(this);
            return treeMenu;
        }

        public PboNodeModel Parent { get; }

        public string Name
        {
            get => this.name;
            private set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public ImageSource Icon => this.icon.Value;

        public TreeMenuModel Menu => this.menu.Value;

        public ObservableCollection<PboNodeModel> Children { get; } = new ObservableCollection<PboNodeModel>();

        protected void AddChild(PboHeaderEntry child, string[] path, int level)
        {
            int currentLevel = level + 1;
            var ctx = new PboNodeModelContext{Name = path[level], Parent = this};
            if (path.Length == currentLevel)
            {
                PboNodeModel file = this.context.GetPboTreeNode(ctx);
                this.Children.Add(file);
            }
            else
            {
                PboNodeModel folder = this.Children.FirstOrDefault(node => node.name == path[level]);
                if (folder == null)
                {
                    folder = this.context.GetPboTreeNode(ctx);
                    this.Children.Add(folder);
                }
                folder.AddChild(child, path, currentLevel);
            }
        }

        public override string ToString()
        {
            string result = $"{{\"Name:\"{this.name}\"}}";
            return result;
        }
    }
}