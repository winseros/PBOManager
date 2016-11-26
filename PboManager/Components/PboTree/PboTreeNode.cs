using System.Collections.Generic;
using System.Windows.Media;
using PboTools.Domain;
using ReactiveUI;

namespace PboManager.Components.PboTree
{
    public abstract class PboTreeNode : ReactiveObject
    {        
        private string name;
        private ImageSource icon;
        private PboTreeNode parent;        

        public PboTreeNode Parent
        {
            get { return this.parent; }
            set { this.RaiseAndSetIfChanged(ref this.parent, value); }
        }

        public ReactiveList<PboTreeNode> Children { get; protected set; }

        public string Name
        {
            get { return this.name; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
        }

        public ImageSource Icon 
        {
            get { return this.icon; }
            set { this.RaiseAndSetIfChanged(ref this.icon, value); }
        }

        public abstract void AddEntry(PboHeaderEntry entry);

        internal abstract PboTreeNode GeOrCreateChildDirectory(string name);

        internal abstract void AddChildFile(string name, PboHeaderEntry entry);

        internal virtual void Sort(IComparer<PboTreeNode> comparer = null)
        {            
        }
    }
}