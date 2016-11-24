using System.Drawing;
using System.Runtime.InteropServices;

namespace PboManager.Services.FileIconService
{
    public class IconReader
    {
        public static Icon GetFileIcon(string name)
        {
            var shfi = new Shell32.Shfileinfo();
            uint flags = Shell32.ShgfiIcon | Shell32.ShgfiSmallicon | Shell32.ShgfiUsefileattributes;
            Shell32.SHGetFileInfo(name, Shell32.FileAttributeNormal, ref shfi, (uint) Marshal.SizeOf(shfi), flags);            
            var icon = (Icon) Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon);
            return icon;
        }

        public static Icon GetDirectoryIcon()
        {
            var shfi = new Shell32.Shfileinfo();
            uint flags = Shell32.ShgfiIcon | Shell32.ShgfiSmallicon | Shell32.ShgfiUsefileattributes;
            Shell32.SHGetFileInfo("directory", Shell32.FileAttributeDirectory, ref shfi, (uint)Marshal.SizeOf(shfi), flags);
            var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon);
            return icon;
        }
    }
}