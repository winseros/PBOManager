using LightInject;
using PboConsole.Commands;

namespace PboConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new ServiceContainer())
            {
                container.SetDefaultLifetime<PerContainerLifetime>();
                container.RegisterFrom<PboTools.CompositionRoot>();
                container.RegisterFrom<CompositionRoot>();

                var core = container.GetInstance<ConsoleCore>();
                IConsoleCommand command = core.GetCommand(args);

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