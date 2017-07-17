using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class DeleteMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public DeleteMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
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
            this.logger.Debug("Deleting the node: \"{0}\"", this.Node);
        }
    }
}
