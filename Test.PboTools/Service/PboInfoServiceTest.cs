using System.IO;
using NUnit.Framework;
using PboTools.Domain;
using PboTools.Service;

namespace Test.PboTools.Service
{
    public class PboInfoServiceTest
    {
        private PboInfoService GetService()
        {
            return new PboInfoService();
        }

        [Test]
        public void Test_ReadPboInfo_Retruns_Pbo_Header_Info_With_Signature_And_Hash()
        {
            using (Stream stream = File.OpenRead(PathUtil.GetPath(@"TestData\signature_3items_packed_hash.pbo")))
            using (var reader = new PboBinaryReader(stream))
            {
                PboInfoService service = this.GetService();
                PboInfo info = service.ReadPboInfo(reader);

                Assert.NotNull(info);

                #region Signature

                Assert.NotNull(info.Signature);
                Assert.AreEqual("", info.Signature.FileName);
                Assert.AreEqual(PboPackingMethod.Product, info.Signature.PackingMethod);
                Assert.AreEqual(0, info.Signature.OriginalSize);
                Assert.AreEqual(0, info.Signature.Reserved);
                Assert.AreEqual(0, info.Signature.TimeStamp);
                Assert.AreEqual(0, info.Signature.DataSize);
                Assert.AreEqual(0, info.Signature.DataOffset);

                #endregion

                #region HeaderExtensions

                Assert.IsNotNull(info.HeaderExtensions);
                Assert.AreEqual(2, info.HeaderExtensions.Count);
                Assert.AreEqual("value1", info.HeaderExtensions["property1"]);
                Assert.AreEqual("value2", info.HeaderExtensions["property2"]);

                #endregion

                #region FileRecords

                Assert.IsNotNull(info.FileRecords);
                Assert.AreEqual(3, info.FileRecords.Count);

                PboHeaderEntry rc1 = info.FileRecords[0];
                Assert.IsNotNull(rc1);
                Assert.AreEqual(PboPackingMethod.Uncompressed, rc1.PackingMethod);
                Assert.AreEqual(15, rc1.OriginalSize);
                Assert.AreEqual(0, rc1.Reserved);
                Assert.AreEqual(1469979348, rc1.TimeStamp);
                Assert.AreEqual(15, rc1.DataSize);
                Assert.AreEqual(167, rc1.DataOffset);

                PboHeaderEntry rc2 = info.FileRecords[1];
                Assert.IsNotNull(rc2);
                Assert.AreEqual(PboPackingMethod.Uncompressed, rc2.PackingMethod);
                Assert.AreEqual(15, rc2.OriginalSize);
                Assert.AreEqual(0, rc2.Reserved);
                Assert.AreEqual(1469979359, rc2.TimeStamp);
                Assert.AreEqual(15, rc2.DataSize);
                Assert.AreEqual(182, rc2.DataOffset);

                PboHeaderEntry rc3 = info.FileRecords[2];
                Assert.IsNotNull(rc3);
                Assert.AreEqual(PboPackingMethod.Uncompressed, rc3.PackingMethod);
                Assert.AreEqual(15, rc3.OriginalSize);
                Assert.AreEqual(0, rc3.Reserved);
                Assert.AreEqual(1469979372, rc3.TimeStamp);
                Assert.AreEqual(15, rc3.DataSize);
                Assert.AreEqual(197, rc3.DataOffset);

                #endregion

                var sha1 = new byte[] {0x45, 0xd1, 0x33, 0x5e, 0x96, 0x6c, 0x5b, 0xd1, 0x27, 0x01, 0x3a, 0xff, 0x28, 0xee, 0x71, 0xe2, 0x49, 0x3f, 0x98, 0x9d};
                CollectionAssert.AreEqual(sha1, info.Checksum);

                Assert.AreEqual(rc1.DataOffset, info.DataBlockStart);
            }
        }

        [Test]
        public void Test_ReadPboInfo_Retruns_Pbo_Header_Info_With_NoSignature_And_NoHash()
        {
            using (Stream stream = File.OpenRead(PathUtil.GetPath(@"TestData\nosignature_3items_packed_nohash.pbo")))
            using (var reader = new PboBinaryReader(stream))
            {
                PboInfoService service = this.GetService();
                PboInfo info = service.ReadPboInfo(reader);

                Assert.NotNull(info);

                #region Signature

                Assert.Null(info.Signature);

                #endregion

                #region HeaderExtensions

                Assert.IsNotNull(info.HeaderExtensions);
                Assert.AreEqual(0, info.HeaderExtensions.Count);

                #endregion

                #region FileRecords

                Assert.IsNotNull(info.FileRecords);
                Assert.AreEqual(3, info.FileRecords.Count);

                #endregion

                Assert.Null(info.Checksum);
            }
        }

        [Test]
        public void Test_ReadPboInfo_Retruns_Pbo_Header_Info_With_NoItems_And_NoHash()
        {
            using (Stream stream = File.OpenRead(PathUtil.GetPath(@"TestData\signature_0items_nohash.pbo")))
            using (var reader = new PboBinaryReader(stream))
            {
                PboInfoService service = this.GetService();
                PboInfo info = service.ReadPboInfo(reader);

                Assert.NotNull(info);

                #region Signature

                Assert.NotNull(info.Signature);

                #endregion

                #region HeaderExtensions

                Assert.IsNotNull(info.HeaderExtensions);
                Assert.AreEqual(2, info.HeaderExtensions.Count);

                #endregion

                #region FileRecords

                Assert.IsNotNull(info.FileRecords);
                Assert.AreEqual(0, info.FileRecords.Count);

                #endregion

                Assert.Null(info.Checksum);
            }
        }
    }
}