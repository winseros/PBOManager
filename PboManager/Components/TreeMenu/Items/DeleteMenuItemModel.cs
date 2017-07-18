using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class DeleteMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public DeleteMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger)
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Delete";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Deleting the node: \"{0}\"", this.node);
        }
    }
}
