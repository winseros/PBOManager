using PboManager.Services.EventBus;
using PboManager.Services.ExceptionService;
using PboManager.Services.OpenFileService;
using PboTools.Service;

namespace PboManager.Components.MainMenu
{
    public interface IMainMenuContext
    {
        IExceptionService GetExceptionService();

        IOpenFileService GetOpenFileService();

        IEventBus GetEventBus();

        IPboArchiverService GetArchiverService();
    }
}
