using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class CutMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public CutMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
           return "Cut";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Cutting the node: \"{0}\"", this.Node);
        }
    }
}
