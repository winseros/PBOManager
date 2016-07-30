using LightInject;
using NLog;
using PboConsole.Commands;
using PboConsole.Services;

namespace PboConsole
{
    public class CompositionRoot : ICompositionRoot
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Compose(IServiceRegistry serviceRegistry)
        {
            logger.Debug("Building the root");

            serviceRegistry.Register<ConsoleCore>();
            serviceRegistry.Register<IConsoleCommand, PackConsoleCommand>("PackConsoleCommand");
            serviceRegistry.Register<IConsoleCommand, UnpackConsoleCommand>("UnpackConsoleCommand");
            serviceRegistry.Register<IConsoleService, ConsoleService>();
        }
    }
}
