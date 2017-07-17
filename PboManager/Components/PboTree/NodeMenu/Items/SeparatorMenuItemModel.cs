namespace PboManager.Components.PboTree.NodeMenu.Items
{
    public class SeparatorMenuItemModel: NodeMenuItemModel
    {
        public SeparatorMenuItemModel(PboNodeModel node)
            : base(node)
        {
        }

        protected override string GetMenuItemText()
        {
           return "-";
        }

        protected override void HandleExecute(object param)
        {
        }
    }
}
