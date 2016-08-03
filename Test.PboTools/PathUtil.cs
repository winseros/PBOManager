using System;
using System.IO;
using System.Reflection;

namespace Test.PboTools
{
    internal static class PathUtil
    {
        private static readonly string codePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        internal static string GetPath(string path)
        {
            string result = Path.Combine(PathUtil.codePath, path);
            return result;
        }
    }
}
