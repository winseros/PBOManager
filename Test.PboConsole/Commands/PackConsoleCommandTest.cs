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
    public class PackConsoleCommandTest
    {
        private IPboArchiverService pboArchiverService;
        private IConsoleService consoleService;

        [SetUp]
        public void SetUp()
        {
            this.pboArchiverService = Substitute.For<IPboArchiverService>();
            this.consoleService = Substitute.For<IConsoleService>();
        }

        private PackConsoleCommand GetCommand()
        {
            return new PackConsoleCommand(this.pboArchiverService, this.consoleService);
        }

        [Test]
        public void Test_TryParse_Returns_False_If_Keyword_Not_Found()
        {
            var args = new[] {"some", "arbitrary", "args", "list"};

            PackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.False(result);
        }

        [Test]
        public void Test_TryParse_Returns_False_If_There_Are_Not_Enough_Args_After_The_Keyword()
        {
            var args = new[] {"some", "arbitrary", "args", PackConsoleCommand.KeyWord};

            PackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.False(result);
        }

        [Test]
        public void Test_TryParse_Returns_True_If_There_Are_1Arg_After_The_Keyword()
        {
            var args = new[] {"some", "arbitrary", "args", PackConsoleCommand.KeyWord, @"C:\PboFolder\PboContents"};

            PackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.True(result);

            NAssert.AreEqual(@"C:\PboFolder\PboContents", command.FolderToPack);
            NAssert.AreEqual(@"PboContents.pbo", command.PboFileName);
        }

        [Test]
        public void Test_TryParse_Returns_True_If_There_Are_2Args_After_The_Keyword()
        {
            var args = new[] {PackConsoleCommand.KeyWord, @"C:\PboFolder\PboContents", @"C:\PboFolder\PboPack.pbo"};

            PackConsoleCommand command = this.GetCommand();
            bool result = command.TryParse(args);

            NAssert.True(result);

            NAssert.AreEqual(@"C:\PboFolder\PboContents", command.FolderToPack);
            NAssert.AreEqual(@"C:\PboFolder\PboPack.pbo", command.PboFileName);
        }


        [Test]
        public void Test_GetUsages_Returns_A_List_Of_Usages()
        {
            PackConsoleCommand command = this.GetCommand();
            IEnumerable<string> usages = command.GetUsages();

            NAssert.NotNull(usages);
            CollectionAssert.AllItemsAreNotNull(usages);
        }


        [Test]
        public void Test_GetSamples_Returns_A_List_Of_Samples()
        {
            PackConsoleCommand command = this.GetCommand();
            IEnumerable<string> samples = command.GetSamples();

            NAssert.NotNull(samples);
            CollectionAssert.AllItemsAreNotNull(samples);
        }


        [Test]
        public void Test_Exec_Runs_The_Archiver_And_Passes_The_Relative_Paths_To_It()
        {
            var folder = @"MyPboPath\MyPboFolder";
            var file = @"MyPboFiles\MyPboFileName.pbo";

            var args = new[] {PackConsoleCommand.KeyWord, folder, file};
            PackConsoleCommand command = this.GetCommand();
            bool parsed = command.TryParse(args);

            NAssert.True(parsed);

            command.Exec();

            string cwd = Directory.GetCurrentDirectory();
            string absFolder = $@"{cwd}\{folder}";
            string absFile = $@"{cwd}\{file}";

            this.pboArchiverService.Received(1).PackDirecoryAsync(absFolder, absFile).IgnoreAwait();
        }

        [Test]
        public void Test_Exec_Runs_The_Archiver_And_Passes_The_Absolute_Paths_To_It()
        {
            var folder = @"C:\MyPboPath\MyPboFolder";
            var file = @"C:\MyPboFiles\MyPboFileName.pbo";

            var args = new[] { PackConsoleCommand.KeyWord, folder, file };
            PackConsoleCommand command = this.GetCommand();
            bool parsed = command.TryParse(args);

            NAssert.True(parsed);

            command.Exec();

            this.pboArchiverService.Received(1).PackDirecoryAsync(folder, file).IgnoreAwait();
        }

        [Test]
        public void Test_Exec_Runs_The_Archiver_And_Passes_The_Normalized_Paths_To_It()
        {
            var folder = @"MyPboPath\MyPboFolder";
            var file = @"MyPboFiles\MyPboFileName.pbo";

            var args = new[] { PackConsoleCommand.KeyWord, $@"..\{folder}", $@"..\{file}" };
            PackConsoleCommand command = this.GetCommand();
            bool parsed = command.TryParse(args);

            NAssert.True(parsed);

            command.Exec();

            string cwd = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string absFolder = $@"{cwd}\{folder}";
            string absFile = $@"{cwd}\{file}";

            this.pboArchiverService.Received(1).PackDirecoryAsync(absFolder, absFile).IgnoreAwait();
        }


        [Test]
        public void Test_ToString_Returns_String_When_No_Fields_Are_Configured()
        {
            PackConsoleCommand command = this.GetCommand();
            string str = command.ToString();

            var expected = "[PboConsole.Commands.PackConsoleCommand]";
            NAssert.AreEqual(expected, str);
        }

        [Test]
        public void Test_ToString_Returns_String_When_FolderToPack_Only_Configured()
        {
            PackConsoleCommand command = this.GetCommand();
            command.TryParse(new[] {PackConsoleCommand.KeyWord, "SomePath\\SomeFolder", ""});
            string str = command.ToString();

            var expected = "[PboConsole.Commands.PackConsoleCommand; FolderToPack=SomePath\\SomeFolder]";
            NAssert.AreEqual(expected, str);
        }

        [Test]
        public void Test_ToString_Returns_String_When_Both_FolderToPack_And_PboFileName_Configured()
        {
            PackConsoleCommand command = this.GetCommand();
            command.TryParse(new[] { PackConsoleCommand.KeyWord, "SomePath\\SomeFolder", "PboFile.pbo" });
            string str = command.ToString();

            var expected = "[PboConsole.Commands.PackConsoleCommand; FolderToPack=SomePath\\SomeFolder; PboFileName=PboFile.pbo]";
            NAssert.AreEqual(expected, str);
        }
    }
}