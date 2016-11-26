using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NLog;

namespace PboManager.Services.FileIconService
{
    public class FileIconServiceImpl : IFileIconService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ImageSource GetFileIcon(string fileExt)
        {
            logger.Debug("Retrieving a file icon for the ext: \"{0}\"", fileExt);

            Icon icon = IconReader.GetFileIcon(fileExt);
            BitmapSource source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            source.Freeze();

            return source;
        }

        public ImageSource GetDirectoryIcon()
        {
            logger.Debug("Retrieving a file icon for a directory");

            Icon icon = IconReader.GetDirectoryIcon();
            BitmapSource source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            source.Freeze();

            return source;
        }
    }
}