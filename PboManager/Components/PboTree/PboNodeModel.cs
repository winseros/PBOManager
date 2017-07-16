using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using PboTools.Domain;

namespace PboManager.Components.PboTree
{
    public class PboNodeModel : ViewModel
    {
        private readonly IPboTreeContext context;

        private string name;
        private ImageSource icon;
        private PboNodeModel parent;

        [Obsolete("For XAML designer")]
        public PboNodeModel()
        {
        }

        public PboNodeModel(IPboTreeContext context)
        {
            this.context = context;
        }

        public PboNodeModel Parent
        {
            get => this.parent;
            set { this.parent = value; this.OnPropertyChanged(); }
        }

        public ObservableCollection<PboNodeModel> Children { get; } = new ObservableCollection<PboNodeModel>();

        public string Name
        {
            get => this.name;
            set { this.name = value; this.OnPropertyChanged(); }
        }

        public ImageSource Icon 
        {
            get => this.icon;
            set { this.icon = value; this.OnPropertyChanged(); }
        }

        protected void AddChild(PboHeaderEntry child, string[] path, int level)
        {
            int currentLevel = level + 1;
            if (path.Length == currentLevel)
            {
                PboNodeModel file = this.context.GetPboTreeNode();
                file.name = path[level];
                file.parent = this;
                file.icon = this.context.GetFileIconService().GetFileIcon(Path.GetExtension(file.name));
                this.Children.Add(file);
            }
            else
            {
                PboNodeModel folder = this.Children.FirstOrDefault(node => node.name == path[level]);
                if (folder == null)
                {
                    folder = this.context.GetPboTreeNode();
                    folder.name = path[level];
                    folder.parent = this;
                    folder.icon = this.context.GetFileIconService().GetDirectoryIcon();
                    this.Children.Add(folder);
                }
                folder.AddChild(child, path, currentLevel);
            }
        }
    }
}