using PboTools.Domain;

namespace PboManager.Components.MainMenu
{
    public class FileOpenedAction
    {
        public string Path { get; set; }

        public PboInfo Pbo { get; set; }
    }
}
