using System;
using System.Collections.Generic;

namespace PboManager.Components.PboTree
{
    public class PboTreeNodeComparer : IComparer<PboTreeNode>
    {
        public int Compare(PboTreeNode x, PboTreeNode y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            bool xIsDirectory = IsDirectory(x);
            bool yIsDirectory = IsDirectory(y);

            if (xIsDirectory != yIsDirectory) return xIsDirectory ? -1 : 1;

            int result = string.Compare(x.Name, y.Name, StringComparison.InvariantCulture);
            return result;
        }

        private static bool IsDirectory(PboTreeNode node)
        {
            bool result = node is PboTreeDirectory;
            return result;
        }
    }
}
