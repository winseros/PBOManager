using System.Collections.Generic;

namespace PboConsole.Commands
{
    public interface IConsoleCommand
    {
        void Exec();

        bool TryParse(string[] args);

        IEnumerable<string> GetUsages();

        IEnumerable<string> GetSamples();
    }
}