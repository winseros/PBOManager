using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.ExceptionService;
using PboTools.Service;

namespace PboManager.Components.MainWindow
{
    public interface IMainWindowContext
    {
        IPboArchiverService GetPboArchiverService();

        IExceptionService GetExceptionService();

        MainMenuModel GetMainMenuModel();

        PboTreeModel GetPboTreeModel();
    }
}
