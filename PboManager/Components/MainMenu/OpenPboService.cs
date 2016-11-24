using Microsoft.Win32;

namespace PboManager.Components.MainMenu
{
    public class OpenPboService : IOpenPboService
    {
        public bool OpenFile(out string fileName)
        {
            var dialog = new OpenFileDialog { Filter = "PBO files|*.pbo|All Files|*" };
            if (dialog.ShowDialog() == true)
            {
                fileName = dialog.FileName;
                return true;
            }

            fileName = null;
            return false;
        }
    }
}