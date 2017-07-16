using System;
using System.Collections.Generic;
using PboTools.Domain;

namespace PboManager.Components.PboTree
{
    public class PboTreeModel : PboNodeModel
    {
        [Obsolete("For XAML designer")]
        public PboTreeModel()
        {
        }

        public PboTreeModel(PboInfo pboInfo, IPboTreeContext context)
            : base(context)
        {            
            this.InflateChildren(pboInfo.FileRecords);
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
