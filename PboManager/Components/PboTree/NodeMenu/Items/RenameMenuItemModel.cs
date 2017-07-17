using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class RenameMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public RenameMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Rename";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Renaming the node: \"{0}\"", this.Node);
        }
    }
}
