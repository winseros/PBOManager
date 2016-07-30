using System.Threading.Tasks;

namespace PboTools.Service
{
    public interface IPboArchiverService
    {
        Task PackDirecoryAsync(string directoryPath, string pboPath);

        Task UnpackPboAsync(string pboPath, string directoryPath);
    }
}
