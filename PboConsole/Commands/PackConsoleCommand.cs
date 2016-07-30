using System;
using System.Collections.Generic;
using System.IO;
using PboTools.Service;

namespace PboConsole.Commands
{
    public class PackConsoleCommand : IConsoleCommand
    {
        internal const string KeyWord = "-pack";

        private readonly IPboArchiverService pboArchiverService;

        private string folderToPack;
        private string pboFileName;

        public PackConsoleCommand(IPboArchiverService pboArchiverService)
        {
            this.pboArchiverService = pboArchiverService;
        }

        public string FolderToPack => this.folderToPack;

        public string PboFileName => this.pboFileName;

        public bool TryParse(string[] args)
        {
            int keywordIndex = Array.IndexOf(args, PackConsoleCommand.KeyWord);
            if (keywordIndex < 0)
                return false;

            int param1Index = keywordIndex + 1;
            int param2Index = keywordIndex + 2;

            if (args.Length <= param1Index)
                return false;

            this.folderToPack = args[param1Index];

            this.pboFileName = (args.Length <= param2Index) ? $"{new DirectoryInfo(this.folderToPack).Name}.pbo" : args[param2Index];

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
            string directoryPath = Path.Combine(cwd, this.folderToPack);
            string pboPath = Path.Combine(cwd, this.pboFileName);

            this.pboArchiverService.PackDirecoryAsync(directoryPath, pboPath).Wait();
        }
    }
}