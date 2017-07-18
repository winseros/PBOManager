using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class ExtractToFolderMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public ExtractToFolderMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger)
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            PboNodeModel item = this.node;
            while (item != item.Parent)
                item = item.Parent;

            return $"Extract to {item.Name}\\ folder";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Extracting the node to the *.pbo folder: \"{0}\"", this.node);
        }
    }
}