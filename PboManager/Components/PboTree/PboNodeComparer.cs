using System;
using System.Collections;
using System.Collections.Generic;

namespace PboManager.Components.PboTree
{
    public class PboNodeComparer : IComparer<PboNodeModel>, IComparer
    {
        public int Compare(PboNodeModel x, PboNodeModel y)
        {
            int result;
            if (x != null && y != null)
            {
                bool isDirectoryX = x.Children.Count == 0;
                bool isDirectoryY = y.Children.Count == 0;

                if (isDirectoryX == isDirectoryY)
                    result = string.Compare(x.Name, y.Name, StringComparison.InvariantCulture);
                else
                    result = isDirectoryX ? 1 : -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public int Compare(object x, object y)
        {
            int result = this.Compare(x as PboNodeModel, y as PboNodeModel);
            return result;
        }
    }
}