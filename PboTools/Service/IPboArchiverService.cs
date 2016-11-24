using System.Threading.Tasks;
using PboTools.Domain;

namespace PboTools.Service
{
    public interface IPboArchiverService
    {
        Task PackDirecoryAsync(string directoryPath, string pboPath);

        Task UnpackPboAsync(string pboPath, string directoryPath);

        PboInfo GetPboInfo(string pboPath);
    }
}
