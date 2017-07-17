using System;
using System.Collections.Generic;
using PboTools.Domain;

namespace PboManager.Components.PboTree
{
    public class PboTreeModel : PboNodeModel
    {
        public PboTreeModel(PboTreeModelContext treeModelContext, IPboTreeContext context)
            : base(new PboNodeModelContext {Name = treeModelContext.FileName}, context)
        {
            this.InflateChildren(treeModelContext.Pbo.FileRecords);
        }

        private void InflateChildren(IEnumerable<PboHeaderEntry> entries)
        {
            foreach (PboHeaderEntry entry in entries)
            {
                if (!entry.FileName.Contains("*"))
                {
                    string[] path = entry.FileName.Split(new[] {"\\"}, StringSplitOptions.RemoveEmptyEntries);
                    this.AddChild(entry, path, 0);
                }
            }
        }
    }
}