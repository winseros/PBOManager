using LightInject;
using PboConsole.Commands;
using PboConsole.Services;

namespace PboConsole
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ConsoleCore>();
            serviceRegistry.Register<IConsoleCommand, PackConsoleCommand>("PackConsoleCommand");
            serviceRegistry.Register<IConsoleCommand, UnpackConsoleCommand>("UnpackConsoleCommand");
            serviceRegistry.Register<IConsoleService, ConsoleService>();
        }
    }
}
