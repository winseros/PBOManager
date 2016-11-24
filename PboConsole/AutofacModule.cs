using Autofac;
using NLog;
using PboConsole.Commands;
using PboConsole.Services;

namespace PboConsole
{
    public class AutofacModule : Module
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            logger.Debug("Building the module");

            builder.RegisterType<ConsoleCore>();
            builder.RegisterType<PackConsoleCommand>().As<IConsoleCommand>();
            builder.RegisterType<UnpackConsoleCommand>().As<IConsoleCommand>();
            builder.RegisterType<ConsoleService>().As<IConsoleService>();
        }
    }
}
