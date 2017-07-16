using System.Threading.Tasks;

namespace Infrastructure
{
    public static class NSubstituteHelper
    {
        public static void IgnoreAwait(this Task task)
        {
        }
    }
}
