using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.EventBus;
using PboTools.Domain;

namespace PboManager.Components.MainWindow
{
    public interface IMainWindowContext
    {
        IEventBus GetEventBus();

        MainMenuModel GetMainMenuModel();

        PboTreeModel GetPboTreeModel(PboInfo pboInfo);

        PboFileModel GetPboFileModel();
    }
}
