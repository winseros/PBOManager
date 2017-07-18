using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.EventBus;

namespace PboManager.Components.MainWindow
{
    public interface IMainWindowContext
    {
        IEventBus GetEventBus();

        MainMenuModel GetMainMenuModel();

        ContextMenuModel GetContextMenuModel(PboNodeModel node);

        PboTreeModel GetPboTreeModel(PboTreeModelContext context);

        PboFileModel GetPboFileModel(PboFileModelContext context);
    }
}
