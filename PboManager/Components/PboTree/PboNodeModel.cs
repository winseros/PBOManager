using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace PboManager.Components.PboTree
{
    public class PboNodeModel : ReactiveObject
    {
        private string nodeName;       
        public string NodeName
        {
            get { return this.nodeName; }
            set { this.RaiseAndSetIfChanged(ref this.nodeName, value); }
        }

        private PboNodeModel parent;       
        public PboNodeModel Parent
        {
            get { return this.parent; }
            set { this.RaiseAndSetIfChanged(ref this.parent, value); }
        }

        public bool IsFolder => this.Children.IsEmpty;
        public IReactiveList<PboNodeModel> Children { get; } = new ReactiveList<PboNodeModel>();
    }
}
