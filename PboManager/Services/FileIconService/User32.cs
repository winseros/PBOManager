using System;
using System.Runtime.InteropServices;

namespace PboManager.Services.FileIconService
{
    internal static class User32
    {        
        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);
    }
}
