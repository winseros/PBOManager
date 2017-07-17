using PboManager.Components.PboTree.NodeMenu;
using PboManager.Services.FileIconService;

namespace PboManager.Components.PboTree
{
    public interface IPboTreeContext
    {
        IFileIconService GetFileIconService();

        PboNodeModel GetPboTreeNode(PboNodeModelContext context);

        NodeMenuModel GetNodeMenu(PboNodeModel node);
    }
}
