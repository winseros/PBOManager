using System.Collections.Generic;
using NLog;
using PboConsole.Services;
using IConsoleCommands = System.Collections.Generic.IEnumerable<PboConsole.Commands.IConsoleCommand>;

namespace PboConsole.Commands
{
    public class ConsoleCore
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IConsoleCommands consoleCommands;
        private readonly IConsoleService consoleService;

        public ConsoleCore(IConsoleCommands consoleCommands, IConsoleService consoleService)
        {
            this.consoleCommands = consoleCommands;
            this.consoleService = consoleService;
        }

        public IConsoleCommand GetCommand(string[] args)
        {
            logger.Debug("Trying to obtain a console command from the arguments");

            IConsoleCommand result = null;
            foreach (var command in this.consoleCommands)
            {
                bool success = command.TryParse(args);
                if (success)
                {
                    logger.Debug("The command reported it can handle the arguments: {0}", command);

                    if (result != null)
                    {
                        logger.Debug("The command reported it can handle the arguments too: {0} - an ambigous situation - exit now", command);

                        result = null;
                        break;
                    }
                    result = command;
                }
            }

            return result;
        }

        public void PrintUsage(string exeFileName)
        {
            logger.Info("Printing usage and samples info using the following fileName: \"{0}\"", exeFileName);

            this.consoleService.Write0TabLine("The genral command syntax is the following:");
            foreach (var command in this.consoleCommands)
            {
                logger.Debug("Printing the usage info for command: \"{0}\"", command.GetType());

                IEnumerable<string> usages = command.GetUsages();
                foreach (var usage in usages)
                {
                    this.consoleService.Write1TabLine($"{exeFileName} {usage}");
                }
                this.consoleService.WriteEmptyLine();
            }
            
            this.consoleService.WriteEmptyLine();
            this.consoleService.Write0TabLine("For example:");
            foreach (var command in this.consoleCommands)
            {
                logger.Debug("Printing the samples for command: \"{0}\"", command.GetType());

                IEnumerable<string> samples = command.GetSamples();
                foreach (var sample in samples)
                {
                    this.consoleService.Write1TabLine($"{exeFileName} {sample}");
                }
                this.consoleService.WriteEmptyLine();
            }
        }
    }
}