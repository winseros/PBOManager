using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class ExtractHereMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public ExtractHereMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            return "Extract to *.pbo folder";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Extracting the node to the *.pbo folder: \"{0}\"", this.Node);
        }
    }
}