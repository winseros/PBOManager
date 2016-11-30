using System;
using System.IO;
using NLog;
using Util;

namespace PboTools.Service
{
    public class TimestampService : ITimestampService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

        public int GetTimestamp(string filePath)
        {
            Assert.NotNull(filePath, nameof(filePath));

            logger.Debug("Obtaining the timestamp for the file: \"{0}\"", filePath);

            DateTime lastWriteTime = File.GetLastWriteTimeUtc(filePath);
            int result = (int)lastWriteTime.Subtract(Date1970).TotalSeconds;

            logger.Debug("The timestamp calculated is: \"{0}\"", result);
            return result;
        }

        public void SetTimestamp(string filePath, int timestamp)
        {
            Assert.NotNull(filePath, nameof(filePath));

            logger.Debug("Setting attributes for the file \"{0}\" using the timestamp \"{1}\"", filePath, timestamp);

            DateTime lastWriteTime = Date1970.Add(TimeSpan.FromSeconds(timestamp));
            File.SetLastWriteTimeUtc(filePath, lastWriteTime);

            logger.Debug("LastWriteTimeUtc has been set to \"{0:O}\"", lastWriteTime);
        }       
    }
}
