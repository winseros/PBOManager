using System.Windows.Media;

namespace PboManager.Services.FileIconService
{
    public interface IFileIconService
    {
        ImageSource GetFileIcon(string fileExt);        

        ImageSource GetDirectoryIcon();
    }
}
