using System;
using System.IO;
using NUnit.Framework;
using PboTools.Service;

namespace Test.PboTools.Service
{
	[TestFixture]
	public class LzhServiceTest
	{
		private LzhService service;

		[SetUp]
		public void BeforeEach()
		{
			this.service = new LzhService();
		}

		[Test]
		public void Test_Decompress_Throws_If_Called_With_Illegal_Args()
		{
			TestDelegate caller = () => this.service.Decompress(null, null, 1);
			var ex1 = Assert.Throws<ArgumentNullException>(caller);
			StringAssert.Contains("source", ex1.Message);

			caller = () => this.service.Decompress(Stream.Null, null, 1);
			ex1 = Assert.Throws<ArgumentNullException>(caller);
			StringAssert.Contains("dest", ex1.Message);

			caller = () => this.service.Decompress(Stream.Null, Stream.Null, 0);
			var ex2 = Assert.Throws<ArgumentOutOfRangeException>(caller);
			StringAssert.Contains("targetLength", ex2.Message);
		}

		[Test]
		public void Test_Decompress_Unpacks_Lzh()
		{
			using (Stream packedData = OpenFile(@"TestData\bjf.paa"))
			using (Stream unpackedData = new MemoryStream())			
			{
				this.service.Decompress(packedData, unpackedData, 18669);
				unpackedData.Position = 0;
				packedData.Position = 0;

				Stream w = File.OpenWrite(@"c:\users\nikita\desktop\1.sqf");
				unpackedData.CopyTo(w);
				w.Flush();
				unpackedData.Position = 0;

				using (Stream targetData = OpenFile(@"TestData\ns_init.uncompressed.sqf"))
				using (var targetReader = new StreamReader(targetData))
				using (var unpackedReader = new StreamReader(unpackedData))
				{
					string targetContent = targetReader.ReadToEnd();
					string unpackedContent = unpackedReader.ReadToEnd();

					Assert.AreEqual(unpackedContent, targetContent);
				}
			}
		}

		private static Stream OpenFile(string fileName)
		{
			string codeBase = typeof(LzhServiceTest).Assembly.CodeBase;
			string folder = Path.GetDirectoryName(codeBase);
			string file = new Uri(Path.Combine(folder, fileName)).AbsolutePath;
			Stream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			return stream;
		}
	}
}
