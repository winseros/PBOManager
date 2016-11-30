using System.IO;
using PboTools.Domain;

namespace PboTools.Service
{
    public interface IPboInfoService
    {
        PboInfo ReadPboInfo(PboBinaryReader reader);

        void WritePboInfo(PboBinaryWriter writer, PboInfo info);

        PboInfo CollectPboInfo(DirectoryInfo directory);

        PboHeaderEntry CollectEntry(string filePath, string entryPath);
    }
}