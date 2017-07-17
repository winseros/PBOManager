using NLog;

namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class ExtractThereMenuItemModel: NodeMenuItemModel
    {
        private readonly IPboTreeContext context;
        private readonly ILogger logger;

        public ExtractThereMenuItemModel(PboNodeModel node, IPboTreeContext context, ILogger logger) 
            : base(node)
        {
            this.context = context;
            this.logger = logger;
        }

        protected override string GetMenuItemText()
        {
            PboNodeModel item = this.Node;
            while (item != item.Parent)
                item = item.Parent;

            return $"Extract to {item.Name}\\ folder";
        }

        protected override void HandleExecute(object param)
        {
            this.logger.Debug("Extracting the node to the *.pbo folder: \"{0}\"", this.Node);
        }
    }
}