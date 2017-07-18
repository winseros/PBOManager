using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class OpenMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public OpenMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Open";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Opening the node: \"{0}\"", this.node);
        }
    }
}
