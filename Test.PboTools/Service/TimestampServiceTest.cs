using System;
using System.IO;
using NUnit.Framework;
using PboTools.Service;

namespace Test.PboTools.Service
{
    public class TimestampServiceTest
    {
        private TimestampService GetService()
        {
            return new TimestampService();
        }

        [Test]
        public void Test_GetTimestamp_Throws_If_Called_With_Illegal_Args()
        {
            TimestampService service = this.GetService();

            TestDelegate caller = () => service.GetTimestamp(null);
            var ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("filePath", ex.Message);
        }

        [Test]
        public void Test_GetTimestamp_Returns_Zero_For_The_1970_Date()
        {
            string path = Path.GetTempFileName();
            File.SetLastWriteTimeUtc(path, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));            

            TimestampService service = this.GetService();
            int timestamp = service.GetTimestamp(path);

            Assert.AreEqual(0, timestamp);
        }

        [Test]
        public void Test_GetTimestamp_Returns_File_Write_Time()
        {
            string path = Path.GetTempFileName();
            File.SetLastWriteTimeUtc(path, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            TimestampService service = this.GetService();
            int timestamp = service.GetTimestamp(path);

            Assert.AreEqual(946684800, timestamp);
        }


        [Test]
        public void Test_SetTimestamp_Throws_If_Called_With_Illegal_Args()
        {
            TimestampService service = this.GetService();

            TestDelegate caller = () => service.SetTimestamp(null, 0);
            var ex = Assert.Catch<ArgumentException>(caller);
            StringAssert.Contains("filePath", ex.Message);
        }

        [Test]
        public void Test_SetTimestamp_Sets_File_Write_Time_To_1970_When_Timestamp_Is_Zero()
        {
            string path = Path.GetTempFileName();

            TimestampService service = this.GetService();
            service.SetTimestamp(path, 0);

            string expected = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O");
            string actual = File.GetLastWriteTimeUtc(path).ToString("O");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SetTimestamp_Sets_File_Write_Time_To_The_Particular_Date()
        {
            string path = Path.GetTempFileName();

            TimestampService service = this.GetService();
            service.SetTimestamp(path, 946684800);

            string expected = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O");
            string actual = File.GetLastWriteTimeUtc(path).ToString("O");

            Assert.AreEqual(expected, actual);
        }        
    }
}
