using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class OpenMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public OpenMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Open";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Opening the node: \"{0}\"", this.Node);
        }
    }
}
