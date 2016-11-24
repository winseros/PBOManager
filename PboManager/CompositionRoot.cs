using LightInject;
using PboManager.Components.MainMenu;
using PboManager.Components.MainWindow;
using PboManager.Components.PboTree;
using PboManager.Services.ExceptionService;

namespace PboManager
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            var container = (IServiceContainer) serviceRegistry;//autofactory hack
            container.RegisterAutoFactory<IMainWindowContext>();            
            serviceRegistry.Register<MainWindowModel>();

            container.RegisterAutoFactory<IMainMenuContext>();
            serviceRegistry.Register<MainMenuModel>();
            serviceRegistry.Register<IOpenPboService, OpenPboService>();            

            serviceRegistry.Register<PboTreeModel>();

            serviceRegistry.Register<IExceptionService, ExceptionServiceImpl>();
        }
    }
}
