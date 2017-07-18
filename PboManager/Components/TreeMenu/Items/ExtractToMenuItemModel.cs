using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class ExtractToMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public ExtractToMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Extract..";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Extracting the node: \"{0}\"", this.node);
        }
    }
}