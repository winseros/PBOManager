using NLog;
using PboManager.Components.PboTree;

namespace PboManager.Components.TreeMenu.Items
{
    public class ExtractToCurrentMenuItemModel: TreeMenuItemModel
    {
        private readonly PboNodeModel node;
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public ExtractToCurrentMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger)
        {
            this.node = node;
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Extract to *.pbo folder";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Extracting the node to the *.pbo folder: \"{0}\"", this.node);
        }
    }
}