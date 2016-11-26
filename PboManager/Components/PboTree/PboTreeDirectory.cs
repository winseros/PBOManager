using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PboManager.Services.FileIconService;
using PboTools.Domain;
using ReactiveUI;
using Util;

namespace PboManager.Components.PboTree
{
    public class PboTreeDirectory : PboTreeNode
    {
        private readonly IPboTreeContext context;
        private static readonly char[] PathSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
        private readonly IDictionary<string, PboTreeNode> childrenDict = new ConcurrentDictionary<string, PboTreeNode>();

        public PboTreeDirectory(IPboTreeContext context)
        {
            this.context = context;

            IFileIconService service = context.GetFileIconService();
            this.Icon = service.GetDirectoryIcon();
            this.Children = new ReactiveList<PboTreeNode>();
        }

        public override void AddEntry(PboHeaderEntry entry)
        {
            Assert.NotNull(entry, nameof(entry));
            Assert.NotNull(entry.FileName, "entry.FileName");
            
            PboTreeNode current = this;
            string[] segments = entry.FileName.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < segments.Length - 1; i++)
                current = current.GeOrCreateChildDirectory(segments[i]);
            
            current.AddChildFile(segments.Last(), entry);
        }

        internal override PboTreeNode GeOrCreateChildDirectory(string name)
        {
            string key = GetDirectorySegment(name);

            PboTreeNode directory;
            if (!this.childrenDict.TryGetValue(key, out directory))
            {
                directory = new PboTreeDirectory(this.context) {Name = name};
                this.childrenDict.Add(key, directory);
                this.Children.Add(directory);
            }

            return directory;
        }

        internal override void AddChildFile(string name, PboHeaderEntry entry)
        {
            string key = GetFileSegment(name);
            var file = new PboTreeFile(this.context)
            {
                Name = name,
                PackingMethod = entry.PackingMethod,
                OriginalSize = entry.OriginalSize,
                Reserved = entry.Reserved,
                TimeStamp = entry.TimeStamp,
                DataSize = entry.TimeStamp,
                DataOffset = entry.DataOffset
            };
            this.childrenDict.Add(key, file);
            this.Children.Add(file);
        }

        internal override void Sort(IComparer<PboTreeNode> comparer = null)
        {
            if (comparer == null)
                comparer = new PboTreeNodeComparer();

            foreach (PboTreeNode child in this.Children)
                child.Sort(comparer);

            this.Children.Sort(comparer);
        }

        private static string GetDirectorySegment(string segment)
        {
            string result = string.Concat("d_", segment);
            return result;
        }

        private static string GetFileSegment(string segment)
        {
            string result = string.Concat("f_", segment);
            return result;
        }
    }
}
