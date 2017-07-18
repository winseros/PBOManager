using Autofac;
using PboManager.Components.TreeMenu.Items;

namespace PboManager.Components.TreeMenu
{
    internal class TreeMenuModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CopyMenuItemModel>().ExternallyOwned();
            builder.RegisterType<CutMenuItemModel>().ExternallyOwned();
            builder.RegisterType<DeleteMenuItemModel>().ExternallyOwned();
            builder.RegisterType<ExtractToCurrentMenuItemModel>().ExternallyOwned();
            builder.RegisterType<ExtractToFolderMenuItemModel>().ExternallyOwned();
            builder.RegisterType<ExtractToMenuItemModel>().ExternallyOwned();
            builder.RegisterType<OpenMenuItemModel>().ExternallyOwned();
            builder.RegisterType<PasteMenuItemModel>().ExternallyOwned();
            builder.RegisterType<RenameMenuItemModel>().ExternallyOwned();
        }
    }
}
