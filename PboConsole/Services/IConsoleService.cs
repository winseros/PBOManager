using System;

namespace PboConsole.Services
{
    public interface IConsoleService
    {
        void WriteEmptyLine();

        void Write0TabLine(string message);

        void Write1TabLine(string message);

        void WriteException(Exception ex);
    }
}
