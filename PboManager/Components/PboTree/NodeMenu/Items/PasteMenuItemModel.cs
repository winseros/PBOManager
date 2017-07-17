using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class PasteMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public PasteMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
           return "Paste";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Pasting content at the node: \"{0}\"", this.Node);
        }
    }
}
