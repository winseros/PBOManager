using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class CopyMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public CopyMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
           return "Copy";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Copying the node: \"{0}\"", this.Node);
        }
    }
}
