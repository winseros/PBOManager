using LightInject;
using NLog;
using PboConsole.Commands;

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
                    command.Exec();
                else
                    core.PrintUsage("PboConsole.exe");
            }


            /*new pboArchiverService().UnpackPboAsync(@"C:\Users\Nikita\Desktop\KingOfTheHill_by_Sa-Matra_for_Bobby.Altis.pbo", "C:\\users\\nikita\\desktop\\king-of-the-hill").Wait();
		    new pboArchiverService().PackDirecoryAsync(@"C:\Users\Nikita\Desktop\king-of-the-hill", @"C:\Users\Nikita\Desktop\ko.pbo").Wait();*/

            /*IConsoleCommand command = ConsoleCore.GetCommand(args);
			command.Exec();*/
        }
    }
}