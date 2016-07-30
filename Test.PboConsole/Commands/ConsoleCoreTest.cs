using NSubstitute;
using NUnit.Framework;
using PboConsole.Commands;
using PboConsole.Services;

namespace Test.PboConsole.Commands
{
    public class ConsoleCoreTest
    {
        private IConsoleService consoleService;

        [SetUp]
        public void SetUp()
        {
            this.consoleService = Substitute.For<IConsoleService>();
        }

        public ConsoleCore GetConsoleCore(params IConsoleCommand[] commands)
        {
            return new ConsoleCore(commands, this.consoleService);
        }

        [Test]
        public void Test_GetCommand_Returns_Null_If_More_Then_One_Command_Fits_Arguments()
        {
            var command1 = Substitute.For<IConsoleCommand>();
            var command2 = Substitute.For<IConsoleCommand>();
            var command3 = Substitute.For<IConsoleCommand>();

            command1.TryParse(null).ReturnsForAnyArgs(true);
            command2.TryParse(null).ReturnsForAnyArgs(true);

            ConsoleCore core = this.GetConsoleCore(command1, command2, command3);

            var args = new[] {"a", "b", "c"};
            IConsoleCommand command = core.GetCommand(args);
            Assert.Null(command);

            command1.Received(1).TryParse(args);
            command2.Received(1).TryParse(args);
            command3.DidNotReceiveWithAnyArgs().TryParse(null);
        }

        [Test]
        public void Test_GetCommand_Returns_Command_If_Only_One_Command_Fits_Arguments()
        {
            var command1 = Substitute.For<IConsoleCommand>();
            var command2 = Substitute.For<IConsoleCommand>();
            var command3 = Substitute.For<IConsoleCommand>();

            command1.TryParse(null).ReturnsForAnyArgs(false);
            command2.TryParse(null).ReturnsForAnyArgs(true);
            command3.TryParse(null).ReturnsForAnyArgs(false);

            ConsoleCore core = this.GetConsoleCore(command1, command2, command3);

            var args = new[] { "a", "b", "c" };
            IConsoleCommand command = core.GetCommand(args);
            Assert.AreSame(command2, command);

            command1.Received(1).TryParse(args);
            command2.Received(1).TryParse(args);
            command3.Received(1).TryParse(args);            
        }


        [Test]
        public void Test_PrintUsage_Prints_The_Command_Usages_And_Samples()
        {
            var command1 = Substitute.For<IConsoleCommand>();
            var command2 = Substitute.For<IConsoleCommand>();

            command1.GetUsages().Returns(new[] {"c1u1", "c1u2"});
            command2.GetUsages().Returns(new[] {"c2u1", "c2u2"});
            command1.GetSamples().Returns(new[] {"c1s1", "c1s2"});
            command2.GetSamples().Returns(new[] {"c2s1", "c2s2"});

            ConsoleCore core = this.GetConsoleCore(command1, command2);
            core.PrintUsage(null);

            this.consoleService.Received(2).Write0TabLine(Arg.Is<string>(p => !string.IsNullOrEmpty(p)));
            this.consoleService.Received(1).Write1TabLine("c1u1");
            this.consoleService.Received(1).Write1TabLine("c1u2");
            this.consoleService.Received(1).Write1TabLine("c2u1");
            this.consoleService.Received(1).Write1TabLine("c2u2");

            this.consoleService.Received(1).Write1TabLine("c1s1");
            this.consoleService.Received(1).Write1TabLine("c1s2");
            this.consoleService.Received(1).Write1TabLine("c2s1");
            this.consoleService.Received(1).Write1TabLine("c2s2");
        }
    }
}