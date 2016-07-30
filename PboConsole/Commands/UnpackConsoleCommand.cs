using System;
using System.Collections.Generic;
using System.IO;
using PboTools.Service;

namespace PboConsole.Commands
{
    public class UnpackConsoleCommand : IConsoleCommand
    {
        internal const string KeyWord = "-unpack";

        private readonly IPboArchiverService pboArchiverService;
        private string pboFileName;
        private string destFolder;

        public UnpackConsoleCommand(IPboArchiverService pboArchiverService)
        {
            this.pboArchiverService = pboArchiverService;
        }

        public string PboFileName => this.pboFileName;

        public string DestFolder => this.destFolder;

        public bool TryParse(string[] args)
        {
            int keywordIndex = Array.IndexOf(args, UnpackConsoleCommand.KeyWord);
            if (keywordIndex < 0)
                return false;

            int param1Index = keywordIndex + 1;
            int param2Index = keywordIndex + 2;

            if (args.Length <= param1Index)
                return false;

            this.pboFileName = args[param1Index];

            this.destFolder = args.Length <= param2Index ? @"\" : args[param2Index];

            return true;
        }

        public IEnumerable<string> GetUsages()
        {
            yield return $"{UnpackConsoleCommand.KeyWord} <fileNameToUnpack>";
            yield return $"{UnpackConsoleCommand.KeyWord} <fileNameToUnpack> <directoryToUnpack>";
        }

        public IEnumerable<string> GetSamples()
        {
            yield return $@"{UnpackConsoleCommand.KeyWord} C:\SomeFolder\PboFile.pbo";
            yield return $@"{UnpackConsoleCommand.KeyWord} PboFile.pbo";
            yield return $@"{UnpackConsoleCommand.KeyWord} ..\PboFile.pbo";
            yield return $@"{UnpackConsoleCommand.KeyWord} C:\SomeFolder\PboFile.pbo C:\SomeFolder\UnpackedPbo";
            yield return $@"{UnpackConsoleCommand.KeyWord} C:\SomeFolder\PboFile.pbo UnpackedPbo";
            yield return $@"{UnpackConsoleCommand.KeyWord} C:\SomeFolder\PboFile.pbo ..\UnpackedPbo";
        }

        public void Exec()
        {
            string cwd = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(cwd, this.PboFileName);
            string destPath = Path.Combine(cwd, this.DestFolder);

            this.pboArchiverService.UnpackPboAsync(filePath, destPath).Wait();
        }
    }
}