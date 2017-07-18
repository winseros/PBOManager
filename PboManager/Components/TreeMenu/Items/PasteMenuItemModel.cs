using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class PasteMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public PasteMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger)
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
           return "Paste";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Pasting content at the node: \"{0}\"", this.node);
        }
    }
}
