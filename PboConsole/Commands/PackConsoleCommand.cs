using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using PboConsole.Services;
using PboTools.Service;

namespace PboConsole.Commands
{
    public class PackConsoleCommand : IConsoleCommand
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        internal const string KeyWord = "-pack";

        private readonly IPboArchiverService pboArchiverService;
        private readonly IConsoleService consoleService;

        private string folderToPack;
        private string pboFileName;

        public PackConsoleCommand(IPboArchiverService pboArchiverService, IConsoleService consoleService)
        {
            this.pboArchiverService = pboArchiverService;
            this.consoleService = consoleService;
        }

        public string FolderToPack => this.folderToPack;

        public string PboFileName => this.pboFileName;

        public bool TryParse(string[] args)
        {
            logger.Debug("Trying to parse arguments");

            int keywordIndex = Array.IndexOf(args, PackConsoleCommand.KeyWord);
            if (keywordIndex < 0)
            {
                logger.Debug("The arguments have no \"{0}\" keyword - exit now", PackConsoleCommand.KeyWord);
                return false;
            }

            logger.Debug("The keyword \"{0}\" has been found at pos: {1}", PackConsoleCommand.KeyWord, keywordIndex);

            int param1Index = keywordIndex + 1;
            int param2Index = keywordIndex + 2;

            if (args.Length <= param1Index)
            {
                logger.Debug("The arguments have not enough elements to proceed - exit now");
                return false;
            }

            this.folderToPack = args[param1Index];
            logger.Debug("Set the {0} property to: \"{1}\"", nameof(this.folderToPack), this.folderToPack);

            this.pboFileName = (args.Length <= param2Index) ? $"{new DirectoryInfo(this.folderToPack).Name}.pbo" : args[param2Index];
            logger.Debug("Set the {0} property to: \"{1}\"", nameof(this.pboFileName), this.pboFileName);

            return true;
        }

        public IEnumerable<string> GetUsages()
        {
            yield return $"{PackConsoleCommand.KeyWord} <directoryToPack>";
            yield return $"{PackConsoleCommand.KeyWord} <directoryToPack> <pboFileName>";
        }

        public IEnumerable<string> GetSamples()
        {
            yield return $@"{PackConsoleCommand.KeyWord} C:\SomeFolder\PboSourceDirectory";
            yield return $@"{PackConsoleCommand.KeyWord} PboSourceDirectory";
            yield return $@"{PackConsoleCommand.KeyWord} ..\PboSourceDirectory";
            yield return $@"{PackConsoleCommand.KeyWord} C:\SomeFolder\PboSourceDirectory pboFile.pbo";
            yield return $@"{PackConsoleCommand.KeyWord} C:\SomeFolder\PboSourceDirectory C:\pboFile.pbo";
            yield return $@"{PackConsoleCommand.KeyWord} C:\SomeFolder\PboSourceDirectory ..\pboFile.pbo";
        }

        public void Exec()
        {           
            string cwd = Directory.GetCurrentDirectory();
            logger.Debug("Executing the command from the cwd: {0}", cwd);

            string directoryPath = new Uri(Path.Combine(cwd, this.folderToPack)).LocalPath;
            string pboPath = new Uri(Path.Combine(cwd, this.pboFileName)).LocalPath;

            this.consoleService.Write0TabLine($"Packing the directory: {directoryPath}");
            this.consoleService.Write0TabLine($"Into the file: {pboPath}");

            this.pboArchiverService.PackDirecoryAsync(directoryPath, pboPath).Wait();

            this.consoleService.Write0TabLine("Done!");
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(base.ToString());

            if (!string.IsNullOrEmpty(this.FolderToPack))
            {
                builder.Append("; ");
                builder.Append(nameof(this.FolderToPack));
                builder.Append("=");
                builder.Append(this.FolderToPack);
            }

            if (!string.IsNullOrEmpty(this.PboFileName))
            {
                builder.Append("; ");
                builder.Append(nameof(this.PboFileName));
                builder.Append("=");
                builder.Append(this.PboFileName);
            }
            builder.Append("]");

            return builder.ToString();
        }
    }
}