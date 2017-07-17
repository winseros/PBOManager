using Autofac;
using Autofac.Extras.AggregateService;
using PboManager.Components.PboTree.NodeMenu;
using PboManager.Components.PboTree.NodeMenu.Items;

namespace PboManager.Components.PboTree
{
    public class PboTreeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CopyMenuItemModel>().ExternallyOwned();
            builder.RegisterType<CutMenuItemModel>().ExternallyOwned();
            builder.RegisterType<DeleteMenuItemModel>().ExternallyOwned();
            builder.RegisterType<ExtractHereMenuItemModel>().ExternallyOwned();
            builder.RegisterType<ExtractThereMenuItemModel>().ExternallyOwned();
            builder.RegisterType<ExtractToMenuItemModel>().ExternallyOwned();
            builder.RegisterType<OpenMenuItemModel>().ExternallyOwned();
            builder.RegisterType<PasteMenuItemModel>().ExternallyOwned();
            builder.RegisterType<RenameMenuItemModel>().ExternallyOwned();
            builder.RegisterType<PboTreeModel>().ExternallyOwned();
            builder.RegisterType<PboNodeModel>().ExternallyOwned();
            builder.RegisterType<NodeMenuModel>().ExternallyOwned();
            builder.RegisterAggregateService<IPboTreeContext>();
        }
    }
}
