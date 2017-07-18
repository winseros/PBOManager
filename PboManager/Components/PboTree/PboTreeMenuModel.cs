using System.Collections.ObjectModel;
using Autofac;
using PboManager.Components.TreeMenu;
using PboManager.Components.TreeMenu.Items;

namespace PboManager.Components.PboTree
{
    public class PboTreeMenuModel : TreeMenuModel
    {
        private readonly PboNodeModel node;
        private readonly ILifetimeScope scope;

        public PboTreeMenuModel(PboNodeModel node, ILifetimeScope scope)
        {
            this.node = node;
            this.scope = scope;
        }

        protected override ObservableCollection<TreeMenuItemModel> GetItems()
        {
            var s = new SeparatorMenuItemModel();
            var param = new TypedParameter(typeof(PboNodeModel), this.node);
            var i1 = this.scope.Resolve<OpenMenuItemModel>(param);
            var i2 = this.scope.Resolve<RenameMenuItemModel>(param);
            var i3 = this.scope.Resolve<ExtractToMenuItemModel>(param);
            var i4 = this.scope.Resolve<ExtractToCurrentMenuItemModel>(param);
            var i5 = this.scope.Resolve<ExtractToFolderMenuItemModel>(param);
            var i6 = this.scope.Resolve<CutMenuItemModel>(param);
            var i7 = this.scope.Resolve<CopyMenuItemModel>(param);
            var i8 = this.scope.Resolve<PasteMenuItemModel>(param);
            var i9 = this.scope.Resolve<DeleteMenuItemModel>(param);
            return new ObservableCollection<TreeMenuItemModel>(new TreeMenuItemModel[] {i1, s, i2, s, i3, i4, i5, s, i6, i7, i8, s, i9});
        }
    }
}
