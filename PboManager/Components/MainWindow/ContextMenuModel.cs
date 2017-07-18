using System.Collections.ObjectModel;
using Autofac;
using PboManager.Components.PboTree;
using PboManager.Components.TreeMenu;
using PboManager.Components.TreeMenu.Items;

namespace PboManager.Components.MainWindow
{
    public class ContextMenuModel : TreeMenuModel
    {
        private readonly PboNodeModel node;
        private readonly ILifetimeScope scope;

        public ContextMenuModel(PboNodeModel node, ILifetimeScope scope)
        {
            this.node = node;
            this.scope = scope;
        }

        protected override ObservableCollection<TreeMenuItemModel> GetItems()
        {
            var param = new TypedParameter(typeof(PboNodeModel), this.node);
            var i1 = this.scope.Resolve<ExtractToMenuItemModel>(param);
            var i2 = this.scope.Resolve<ExtractToCurrentMenuItemModel>(param);
            var i3 = this.scope.Resolve<ExtractToFolderMenuItemModel>(param);
            return new ObservableCollection<TreeMenuItemModel>(new TreeMenuItemModel[] {i1, i2, i3 });
        }
    }
}
