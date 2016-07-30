using System.Collections.Generic;
using PboConsole.Services;
using IConsoleCommands = System.Collections.Generic.IEnumerable<PboConsole.Commands.IConsoleCommand>;

namespace PboConsole.Commands
{
    public class ConsoleCore
    {
        private readonly IConsoleCommands consoleCommands;
        private readonly IConsoleService consoleService;

        public ConsoleCore(IConsoleCommands consoleCommands, IConsoleService consoleService)
        {
            this.consoleCommands = consoleCommands;
            this.consoleService = consoleService;
        }

        public IConsoleCommand GetCommand(string[] args)
        {
            IConsoleCommand result = null;
            foreach (var command in this.consoleCommands)
            {
                bool success = command.TryParse(args);
                if (success)
                {
                    if (result != null)
                    {
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
            this.consoleService.Write0TabLine("The genral command syntax is the following:");
            foreach (var command in this.consoleCommands)
            {
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