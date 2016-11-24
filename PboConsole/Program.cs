using System;
using System.Linq;
using Autofac;
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

            var builder = new ContainerBuilder();
            builder.RegisterModule<PboTools.AutofacModule>();
            builder.RegisterModule<AutofacModule>();

            using (IContainer container = builder.Build())
            {
                logger.Debug("Container configuration complete");

                var core = container.Resolve<ConsoleCore>();
                IConsoleCommand command = core.GetCommand(args);

                logger.Info("The following command has been chosen according to the input args: {0}", command);

                if (command != null)
                    Program.TryExec(command, container);
                else
                    core.PrintUsage("PboConsole.exe");

                logger.Info("Sucessfully closing the application");
            }
        }

        private static void TryExec(IConsoleCommand command, IContainer container)
        {
            try
            {
                command.Exec();
            }
            catch (AggregateException ex)
            {
                logger.Fatal(ex);

                var inner = ex.InnerExceptions.First();
                var console = container.Resolve<IConsoleService>();
                console.WriteException(inner);
            }
        }
    }
}