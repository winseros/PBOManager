using NUnit.Framework;
using PboTools.Domain;

namespace Test.PboTools.Domain
{
    public class PboHeaderEntryTest
    {
        [Test]
        public void Test_SizeOf_Returns_A_Valid_Object_Size()
        {
            var entry = new PboHeaderEntry();
            int size = PboHeaderEntry.SizeOf(entry);
            Assert.AreEqual(size, 21);

            entry.FileName = "1234567";
            size = PboHeaderEntry.SizeOf(entry);
            Assert.AreEqual(size, 28);
        }

        [Test]
        public void Test_CreateBoundary_Returns_A_New_Boundary_Entry()
        {
            PboHeaderEntry entry1 = PboHeaderEntry.CreateBoundary();
            Assert.NotNull(entry1);
            Assert.True(entry1.IsBoundary);

            Assert.AreEqual(string.Empty, entry1.FileName);
            Assert.AreEqual(PboPackingMethod.Uncompressed, entry1.PackingMethod);

            Assert.AreEqual(0, entry1.OriginalSize);
            Assert.AreEqual(0, entry1.Reserved);
            Assert.AreEqual(0, entry1.TimeStamp);
            Assert.AreEqual(0, entry1.DataSize);
            Assert.AreEqual(0, entry1.DataOffset);

            PboHeaderEntry entry2 = PboHeaderEntry.CreateBoundary();
            Assert.AreNotSame(entry1, entry2);
        }

        [Test]
        public void Test_CreateSignature_Returns_A_New_Signature_Entry()
        {
            PboHeaderEntry entry1 = PboHeaderEntry.CreateSignature();
            Assert.NotNull(entry1);
            Assert.True(entry1.IsSignature);

            Assert.AreEqual(string.Empty, entry1.FileName);
            Assert.AreEqual(PboPackingMethod.Product, entry1.PackingMethod);

            Assert.AreEqual(0, entry1.OriginalSize);
            Assert.AreEqual(0, entry1.Reserved);
            Assert.AreEqual(0, entry1.TimeStamp);
            Assert.AreEqual(0, entry1.DataSize);
            Assert.AreEqual(0, entry1.DataOffset);

            PboHeaderEntry entry2 = PboHeaderEntry.CreateSignature();
            Assert.AreNotSame(entry1, entry2);
        }


        [Test]
        public void Test_IsCompressed_Returns_True()
        {
            var entry = new PboHeaderEntry();
            entry.PackingMethod = PboPackingMethod.Packed;
            entry.OriginalSize = 1;
            entry.OriginalSize = 2;

            Assert.True(entry.IsCompressed);
        }

        [Test]
        public void Test_IsCompressed_Returns_False()
        {
            var entry = new PboHeaderEntry();          
            Assert.False(entry.IsCompressed);

            entry.PackingMethod = PboPackingMethod.Packed;
            Assert.False(entry.IsCompressed);

            entry.OriginalSize = 100;
            entry.DataSize = 100;
            Assert.False(entry.IsCompressed);
        }

        [Test]
        public void Test_GetShortFileName_Returns_A_Short_File_Name()
        {
            var entry = new PboHeaderEntry
            {
                FileName = @"\client\scripts\function.sqf"
            };

            string expected = entry.GetShortFileName();
            Assert.AreEqual("function.sqf", expected);
        }

        [Test]
        public void Test_ToString_Returns_A_Valid_String()
        {
            var entry = new PboHeaderEntry
            {
                PackingMethod = PboPackingMethod.Packed,
                FileName = @"\client\scripts\function.sqf",
                OriginalSize = 101,
                Reserved = 102,
                TimeStamp = 103,
                DataSize = 104,
                DataOffset = 105
            };

            string expected = "FileName:\"\\client\\scripts\\function.sqf\";PackingMethod:Packed;OriginalSize:101;Timestamp:103;DataSize:104;DataOffset:105";
            string str = entry.ToString();
            Assert.AreEqual(expected, str);
        }
    }
}
