using PboManager.Services.FileIconService;
using PboTools.Domain;

namespace PboManager.Components.PboTree
{
    public interface IPboTreeContext
    {
        PboNodeModel GetPboNodeModel(PboHeaderEntry entry);

        IFileIconService GetFileIconService();
    }
}
