using System.Threading.Tasks;

namespace Util
{
    public static class NSubstituteHelper
    {
        public static void IgnoreAwait(this Task task)
        {
        }
    }
}
