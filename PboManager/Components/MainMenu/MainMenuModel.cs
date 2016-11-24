using System.Reactive;
using ReactiveUI;

namespace PboManager.Components.MainMenu
{
    public class MainMenuModel : ReactiveObject
    {
        private readonly IMainMenuContext context;
        private string currentFileName;

        public MainMenuModel(IMainMenuContext context)
        {
            this.context = context;
            this.OpenFileCommand = ReactiveCommand.Create(this.OpenFile);            
        }

        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        
        public string CurrentFileName
        {
            get { return this.currentFileName; }
            set { this.RaiseAndSetIfChanged(ref this.currentFileName, value); }
        }
        
        private void OpenFile()
        {
            IOpenPboService service = this.context.GetOpenPboService();
            string fileName;
            if (service.OpenFile(out fileName))
                this.CurrentFileName = fileName;
        }
    }    
}
