using System;
using System.Runtime.InteropServices;

namespace PboManager.Services.FileIconService
{
    internal class Shell32
    {
        private const int MaxPath = 256;

        [StructLayout(LayoutKind.Sequential)]
        public struct Shfileinfo
        {
            private const int Namesize = 80;

            public readonly IntPtr hIcon;

            private readonly int iIcon;

            private readonly uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
            private readonly string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Namesize)]
            private readonly string szTypeName;
        }

        internal const uint ShgfiIcon = 0x000000100; // get icon
        internal const uint ShgfiLinkoverlay = 0x000008000; // put a link overlay on icon
        internal const uint ShgfiLargeicon = 0x000000000; // get large icon
        internal const uint ShgfiSmallicon = 0x000000001; // get small icon
        internal const uint ShgfiUsefileattributes = 0x000000010; // use passed dwFileAttribute
        internal const uint FileAttributeNormal = 0x00000080;
        internal const uint FileAttributeDirectory = 0x00000010;

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref Shfileinfo psfi, uint cbFileInfo, uint uFlags);
    }
}