using System.Collections.Generic;
using System.IO;
using Infrastructure;
using NSubstitute;
using NUnit.Framework;
using PboConsole.Commands;
using PboConsole.Services;
using PboTools.Service;
using NAssert = NUnit.Framework.Assert;

namespace Test.PboConsole.Commands
{
    public class UnpackConsoleCommandTest
    {
        private IPboArchiverService pboArchiverService;
        private IConsoleService consoleService;

        [SetUp]
        public void SetUp()
        {
            this.pboArchiverService = Substitute.For<IPboArchiverService>();
            this.consoleService = Substitute.For<IConsoleService>();
        }

        private UnpackConsoleCommand GetCommand()
        {
            return new UnpackConsoleCommand(this.pboArchiverService, this.consoleService);
        }

        [Test]
        public void Test_TryParse_Returns_False_If_Keyword_Not_Found()
        {
            var args = new[] { "some", "arbitrary", "args", "list" };

            UnpackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.False(result);
        }

        [Test]
        public void Test_TryParse_Returns_False_If_There_Are_Not_Enough_Args_After_The_Keyword()
        {
            var args = new[] { "some", "arbitrary", "args", UnpackConsoleCommand.KeyWord };

            UnpackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.False(result);
        }

        [Test]
        public void Test_TryParse_Returns_True_If_There_Are_1Arg_After_The_Keyword()
        {
            var args = new[] { "some", "arbitrary", "args", UnpackConsoleCommand.KeyWord, @"C:\PboFolder\PboFile.pbo" };

            UnpackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.True(result);

            NAssert.AreEqual(@"C:\PboFolder\PboFile.pbo", command.PboFileName);
            NAssert.AreEqual(@"PboFile", command.DestFolder);
        }

        [Test]
        public void Test_TryParse_Returns_True_If_There_Are_2Args_After_The_Keyword()
        {
            var args = new[] { UnpackConsoleCommand.KeyWord, @"C:\PboFolder\PboFile.pbo", @"C:\PboFolder\PboUnpacked" };

            UnpackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.True(result);

            NAssert.AreEqual(@"C:\PboFolder\PboFile.pbo", command.PboFileName);
            NAssert.AreEqual(@"C:\PboFolder\PboUnpacked", command.DestFolder);
        }


        [Test]
        public void Test_GetUsages_Returns_A_List_Of_Usages()
        {
            UnpackConsoleCommand command = this.GetCommand();
            IEnumerable<string> usages = command.GetUsages();

            NAssert.NotNull(usages);
            CollectionAssert.AllItemsAreNotNull(usages);
        }


        [Test]
        public void Test_GetSamples_Returns_A_List_Of_Samples()
        {
            UnpackConsoleCommand command = this.GetCommand();
            IEnumerable<string> samples = command.GetSamples();

            NAssert.NotNull(samples);
            CollectionAssert.AllItemsAreNotNull(samples);
        }


        [Test]
        public void Test_Exec_Runs_The_Archiver_And_Passes_The_Relative_Paths_To_It()
        {
            var file = @"MyPboFiles\MyPboFileName.pbo";
            var folder = @"MyPboPath\MyPboFolder";
            
            var args = new[] { UnpackConsoleCommand.KeyWord, file, folder };
            UnpackConsoleCommand command = this.GetCommand();
            bool parsed = command.TryParse(args);

            NAssert.True(parsed);

            command.Exec();

            string cwd = Directory.GetCurrentDirectory();
            string absFile = $@"{cwd}\{file}";
            string absFolder = $@"{cwd}\{folder}";
            
            this.pboArchiverService.Received(1).UnpackPboAsync(absFile, absFolder).IgnoreAwait();
        }

        [Test]
        public void Test_Exec_Runs_The_Archiver_And_Passes_The_Absolute_Paths_To_It()
        {
            var file = @"C:\MyPboFiles\MyPboFileName.pbo";
            var folder = @"C:\MyPboPath\MyPboFolder";

            var args = new[] { UnpackConsoleCommand.KeyWord, file, folder };
            UnpackConsoleCommand command = this.GetCommand();
            bool parsed = command.TryParse(args);

            NAssert.True(parsed);

            command.Exec();

            this.pboArchiverService.Received(1).UnpackPboAsync(file, folder).IgnoreAwait();
        }

        [Test]
        public void Test_Exec_Runs_The_Archiver_And_Passes_The_Normalized_Paths_To_It()
        {
            var file = @"MyPboFiles\MyPboFileName.pbo";
            var folder = @"MyPboPath\MyPboFolder";

            var args = new[] { UnpackConsoleCommand.KeyWord, $@"..\{file}", $@"..\{folder}" };
            UnpackConsoleCommand command = this.GetCommand();
            bool parsed = command.TryParse(args);

            NAssert.True(parsed);

            command.Exec();

            string cwd = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string absFile = $@"{cwd}\{file}";
            string absFolder = $@"{cwd}\{folder}";

            this.pboArchiverService.Received(1).UnpackPboAsync(absFile, absFolder).IgnoreAwait();
        }


        [Test]
        public void Test_ToString_Returns_String_When_No_Fields_Are_Configured()
        {
            UnpackConsoleCommand command = this.GetCommand();
            string str = command.ToString();

            var expected = "[PboConsole.Commands.UnpackConsoleCommand]";
            NAssert.AreEqual(expected, str);
        }

        [Test]
        public void Test_ToString_Returns_String_When_PboFileName_Only_Configured()
        {
            UnpackConsoleCommand command = this.GetCommand();
            command.TryParse(new[] { UnpackConsoleCommand.KeyWord, "PboFile.pbo", "" });
            string str = command.ToString();

            var expected = "[PboConsole.Commands.UnpackConsoleCommand; PboFileName=PboFile.pbo]";
            NAssert.AreEqual(expected, str);
        }

        [Test]
        public void Test_ToString_Returns_String_When_Both_PboFileName_And_DestFolder_Configured()
        {
            UnpackConsoleCommand command = this.GetCommand();
            command.TryParse(new[] { UnpackConsoleCommand.KeyWord, "PboFile.pbo", "SomePath\\SomeFolder" });
            string str = command.ToString();

            var expected = "[PboConsole.Commands.UnpackConsoleCommand; PboFileName=PboFile.pbo; DestFolder=SomePath\\SomeFolder]";
            NAssert.AreEqual(expected, str);
        }
    }
}
