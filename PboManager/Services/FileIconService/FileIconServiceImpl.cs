using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Util;

namespace PboManager.Services.FileIconService
{
    public class FileIconServiceImpl : IFileIconService
    {
        private readonly ConcurrentDictionary<string, WeakReference<ImageSource>> cache = new ConcurrentDictionary<string, WeakReference<ImageSource>>();

        public ImageSource GetFileIcon(string fileExt)
        {
            Assert.NotNull(fileExt, nameof(fileExt));

            ImageSource source;
            if (!this.TryGetFromCache(fileExt, out source))
            {
                Icon icon = IconReader.GetFileIcon(fileExt);
                source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                source.Freeze();

                this.PutToCache(fileExt, source);
            }
            return source;
        }

        public ImageSource GetDirectoryIcon()
        {
            const string directoryKey = "directory";
            ImageSource source;
            if (!this.TryGetFromCache(directoryKey, out source))
            {
                Icon icon = IconReader.GetDirectoryIcon();
                source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                this.PutToCache(directoryKey, source);
            }
            return source;
        }

        private bool TryGetFromCache(string key, out ImageSource source)
        {
            WeakReference<ImageSource> container;
            if (this.cache.TryGetValue(key, out container))
            {
                bool result = container.TryGetTarget(out source);
                return result;
            }

            source = null;
            return false;
        }

        private void PutToCache(string key, ImageSource source)
        {
            WeakReference<ImageSource> container;
            if (this.cache.TryGetValue(key, out container))
            {
                container.SetTarget(source);
            }
            else
            {
                container = new WeakReference<ImageSource>(source);
                this.cache.AddOrUpdate(key, container, (s, reference) => reference);
            }
        }
    }
}