using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using PboTools.Service;

namespace PboConsole.Commands
{
    public class UnpackConsoleCommand : IConsoleCommand
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Debug("Trying to parse arguments");

            int keywordIndex = Array.IndexOf(args, UnpackConsoleCommand.KeyWord);
            if (keywordIndex < 0)
            {
                logger.Debug("The arguments have no \"{0}\" keyword - exit now", PackConsoleCommand.KeyWord);
                return false;
            }

            int param1Index = keywordIndex + 1;
            int param2Index = keywordIndex + 2;

            if (args.Length <= param1Index)
            {
                logger.Debug("The arguments have not enough elements to proceed - exit now");
                return false;
            }

            this.pboFileName = args[param1Index];
            logger.Debug("Set the {0} property to: \"{1}\"", nameof(this.pboFileName), this.pboFileName);

            this.destFolder = args.Length <= param2Index ? @"\" : args[param2Index];
            logger.Debug("Set the {0} property to: \"{1}\"", nameof(this.destFolder), this.destFolder);

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
            logger.Info("Executing the command");

            string cwd = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(cwd, this.PboFileName);
            string destPath = Path.Combine(cwd, this.DestFolder);

            this.pboArchiverService.UnpackPboAsync(filePath, destPath).Wait();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(base.ToString());

            if (!string.IsNullOrEmpty(this.PboFileName))
            {
                builder.Append("; ");
                builder.Append(nameof(this.PboFileName));
                builder.Append("=");
                builder.Append(this.PboFileName);
            }

            if (!string.IsNullOrEmpty(this.DestFolder))
            {
                builder.Append("; ");
                builder.Append(nameof(this.DestFolder));
                builder.Append("=");
                builder.Append(this.DestFolder);
            }
            builder.Append("]");

            return builder.ToString();
        }
    }
}