using Microsoft.Win32;

namespace PboManager.Services.OpenFileService
{
    public class OpenFileServiceImpl : IOpenFileService
    {
        public string OpenFile()
        {
            var dialog = new OpenFileDialog {Filter = "PBO files|*.pbo|All Files|*"};
            string fileName = dialog.ShowDialog() == true ? dialog.FileName : null;
            return fileName;
        }
    }
}