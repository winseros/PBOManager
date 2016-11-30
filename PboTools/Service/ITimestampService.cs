using System.IO;

namespace PboTools.Service
{
    public interface ITimestampService
    {
        int GetTimestamp(string filePath);

        void SetTimestamp(string filePath, int timestamp);
    }
}