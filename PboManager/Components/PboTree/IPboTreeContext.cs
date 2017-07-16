using PboManager.Services.FileIconService;

namespace PboManager.Components.PboTree
{
    public interface IPboTreeContext
    {
        IFileIconService GetFileIconService();

        PboNodeModel GetPboTreeNode();
    }
}
