using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class RenameMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public RenameMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Rename";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Renaming the node: \"{0}\"", this.node);
        }
    }
}
