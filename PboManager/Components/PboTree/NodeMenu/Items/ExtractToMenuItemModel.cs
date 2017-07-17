using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class ExtractToMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public ExtractToMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Extract..";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Extracting the node: \"{0}\"", this.Node);
        }
    }
}