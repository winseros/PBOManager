using System;
using System.Linq;
using LightInject;
using NLog;
using PboConsole.Commands;
using PboConsole.Services;

namespace PboConsole
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Started the console application");

            using (var container = new ServiceContainer())
            {
                container.SetDefaultLifetime<PerContainerLifetime>();
                container.RegisterFrom<PboTools.CompositionRoot>();
                container.RegisterFrom<CompositionRoot>();

                logger.Debug("Container configuration complete");

                var core = container.GetInstance<ConsoleCore>();
                IConsoleCommand command = core.GetCommand(args);

                logger.Info("The following command has been chosen according to the input args: {0}", command);

                if (command != null)
                    Program.TryExec(command, container);
                else
                    core.PrintUsage("PboConsole.exe");

                logger.Info("Sucessfully closing the application");
            }
        }

        private static void TryExec(IConsoleCommand command, IServiceContainer container)
        {
            try
            {
                command.Exec();
            }
            catch (AggregateException ex)
            {
                logger.Fatal(ex);

                var inner = ex.InnerExceptions.First();
                var console = container.GetInstance<IConsoleService>();
                console.WriteException(inner);
            }
        }
    }
}