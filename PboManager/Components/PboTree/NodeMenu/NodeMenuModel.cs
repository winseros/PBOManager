using System;
using System.Collections.ObjectModel;
using System.Threading;
using Autofac;
using PboManager.Components.PboTree.NodeMenu.Items;

namespace PboManager.Components.PboTree.NodeMenu
{
    public class NodeMenuModel : ViewModel
    {
        private readonly PboNodeModel node;
        private readonly ILifetimeScope scope;
        private readonly Lazy<ObservableCollection<NodeMenuItemModel>> items;

        public NodeMenuModel(PboNodeModel node, ILifetimeScope scope)
        {
            this.node = node;
            this.scope = scope;
            this.items = new Lazy<ObservableCollection<NodeMenuItemModel>>(this.GetItems, LazyThreadSafetyMode.None);
        }

        private ObservableCollection<NodeMenuItemModel> GetItems()
        {
            var s = new SeparatorMenuItemModel(null);
            var param = new TypedParameter(typeof(PboNodeModel), this.node);
            var i1 = this.scope.Resolve<OpenMenuItemModel>(param);
            var i2 = this.scope.Resolve<RenameMenuItemModel>(param);
            var i3 = this.scope.Resolve<ExtractToMenuItemModel>(param);
            var i4 = this.scope.Resolve<ExtractHereMenuItemModel>(param);
            var i5 = this.scope.Resolve<ExtractThereMenuItemModel>(param);
            var i6 = this.scope.Resolve<CutMenuItemModel>(param);
            var i7 = this.scope.Resolve<CopyMenuItemModel>(param);
            var i8 = this.scope.Resolve<PasteMenuItemModel>(param);
            var i9 = this.scope.Resolve<DeleteMenuItemModel>(param);
            return new ObservableCollection<NodeMenuItemModel>(new NodeMenuItemModel[] {i1, s, i2, s, i3, i4, i5, s, i6, i7, i8, s, i9});
        }

        public ObservableCollection<NodeMenuItemModel> Items => this.items.Value;
    }
}