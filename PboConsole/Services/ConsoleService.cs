using System;
using Util;

namespace PboConsole.Services
{
    public class ConsoleService : IConsoleService
    {
        public void WriteEmptyLine()
        {
            Console.WriteLine();  
        }

        public void Write0TabLine(string message)
        {
            Assert.NotNull(message, nameof(message));
            Console.WriteLine(message);
        }

        public void Write1TabLine(string message)
        {
            Assert.NotNull(message, nameof(message));
            Console.Write("\t");
            Console.WriteLine(message);
        }
    }
}