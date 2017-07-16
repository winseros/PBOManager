using System.Globalization;
using AutofacContrib.NSubstitute;
using NUnit.Framework;
using PboManager.Components.MainWindow;

namespace Test.PboManager.Components.MainWindow
{
    public class WindowTitleConverterTest
    {
        [Test]
        public void Test_Convert_Returns_App_Name_If_Current_File_Is_Null()
        {
            object text = WindowTitleConverter.NSTANCE.Convert(null, null, null, CultureInfo.CurrentCulture);
            Assert.AreEqual("PBOManager", text);
        }

        [Test]
        public void Test_Convert_Returns_Current_File_Name()
        {
            var substitute = new AutoSubstitute();
            var file = substitute.Resolve<PboFileModel>();
            file.Path = @"c:\myfile.pbo";

            object text = WindowTitleConverter.NSTANCE.Convert(file, null, null, CultureInfo.CurrentCulture);
            Assert.AreEqual("myfile.pbo - PBOManager", text);
        }
    }
}
