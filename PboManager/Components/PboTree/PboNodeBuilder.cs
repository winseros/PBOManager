using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PboTools.Domain;
using Util;

namespace PboManager.Components.PboTree
{
    public class PboNodeBuilder : IComparer<PboNodeModel>
    {
        private static readonly char[] PathSeparators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
        private readonly IDictionary<string, PboNodeBuilder> children = new Dictionary<string, PboNodeBuilder>();
        private readonly string name;
        private readonly PboHeaderEntry entry;

        public PboNodeBuilder(string name, PboHeaderEntry entry = null) 
        {
            Assert.NotNull(name, nameof(name));
            this.name = name;
            this.entry = entry;
        }

        public PboNodeBuilder AddEntry(PboHeaderEntry headerEntry)
        {
            string[] segments = headerEntry.FileName.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);

            PboNodeBuilder current = this;
            for (var i = 0; i < segments.Length - 1; i++)
            {
                string segment = segments[i];
                PboNodeBuilder node;
                if (!current.children.TryGetValue(segment, out node))
                {
                    node = new PboNodeBuilder(segment);
                    current.children.Add(segment, node);
                }
                current = node;
            }

            string fileName = segments.Last();
            var leaf = new PboNodeBuilder(fileName, headerEntry);
            current.children.Add(fileName, leaf);

            return this;
        }

        public PboNodeModel Build(IPboTreeContext context)
        {
            Assert.NotNull(context, nameof(context));

            PboNodeModel model = context.GetPboNodeModel(this.entry);
            model.NodeName = this.name;
            model.Initialize();
            
            foreach (PboNodeBuilder value in this.children.Values)
            {
                PboNodeModel child = value.Build(context);
                model.Children.Add(child);                
            }

            model.Children.Sort(this);

            return model;
        }

        #region IComparer

        public int Compare(PboNodeModel x, PboNodeModel y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x.IsDirectory != y.IsDirectory) return x.IsDirectory ? -1 : 1;

            int result = string.Compare(x.NodeName, y.NodeName, StringComparison.InvariantCulture);
            return result;
        }

        #endregion
    }
}