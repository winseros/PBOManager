using System;
using System.Collections.Concurrent;
using System.Windows.Media;
using NLog;
using Util;

namespace PboManager.Services.FileIconService
{
    public class CachingFileIconServiceImpl : IFileIconService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<string, WeakReference<ImageSource>> cache = new ConcurrentDictionary<string, WeakReference<ImageSource>>();
        private readonly IFileIconService service;

        public CachingFileIconServiceImpl(IFileIconService service)
        {
            this.service = service;
        }
       
        public ImageSource GetFileIcon(string fileExt)
        {
            Assert.NotNull(fileExt, nameof(fileExt));

            logger.Debug("Retrieving a file icon for the ext: \"{0}\"", fileExt);

            ImageSource source;
            if (!this.TryGetFromCache(fileExt, out source))
            {
                logger.Debug("No cached data been found - retrieving an icon");

                source = this.service.GetFileIcon(fileExt);
                this.PutToCache(fileExt, source);
            }
            return source;
        }

        public ImageSource GetDirectoryIcon()
        {
            logger.Debug("Retrieving a file icon for a directory");

            const string directoryKey = "directory";
            ImageSource source;
            if (!this.TryGetFromCache(directoryKey, out source))
            {
                logger.Debug("No cached data been found - retrieving an icon");

                source = this.service.GetDirectoryIcon();
                this.PutToCache(directoryKey, source);
            }
            return source;
        }

        private bool TryGetFromCache(string key, out ImageSource source)
        {
            WeakReference<ImageSource> container;
            if (this.cache.TryGetValue(key, out container))
            {
                logger.Debug("Found a reference into the cache by key: \"{0}\"", key);

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
                logger.Debug("Put the icon to an existing container by key: \"{0}\"", key);
                container.SetTarget(source);
            }
            else
            {
                logger.Debug("Creating a new cotainer for the icon by key: \"{0}\"", key);
                container = new WeakReference<ImageSource>(source);
                this.cache.AddOrUpdate(key, container, (s, reference) => reference);
            }
        }
    }
}
