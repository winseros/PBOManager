using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.EventBus;

namespace PboManager.Components.MainWindow
{
    public class MainWindowModel : ViewModel
    {
        private readonly IMainWindowContext context;
        private PboFileModel currentFile;

        public MainWindowModel(IMainWindowContext context)
        {
            this.context = context;
            this.MainMenu = context.GetMainMenuModel();

            IEventBus eventBus = this.context.GetEventBus();
            eventBus.Subscribe<FileOpenedAction>(this.HandleFileOpenedAction);
            eventBus.Subscribe<FileCloseAction>(this.HandleFileCloseAction);
        }

        public MainMenuModel MainMenu { get; }

        public ObservableCollection<PboFileModel> Files { get; } = new ObservableCollection<PboFileModel>();

        public PboFileModel CurrentFile
        {
            get => this.currentFile;
            set
            {
                this.currentFile = value;
                this.OnPropertyChanged();

                var action = new CurrentFileChangedAction {File = value};
                this.context.GetEventBus().Publish(action);
            }
        }

        private void HandleFileOpenedAction(FileOpenedAction action)
        {
            PboFileModel file = this.Files.FirstOrDefault(p => p.Path == action.Path);
            if (file == null)
            {
                var treeModelContext = new PboTreeModelContext
                {
                    FileName = Path.GetFileName(action.Path),
                    Pbo = action.Pbo
                };
                PboTreeModel tree = this.context.GetPboTreeModel(treeModelContext);

                var fileModelContext = new PboFileModelContext
                {
                    Path = action.Path,
                    Tree = tree
                };
                file = this.context.GetPboFileModel(fileModelContext);
                this.Files.Add(file);
            }
            this.CurrentFile = file;
        }

        private void HandleFileCloseAction(FileCloseAction action)
        {
            int index = -1;
            if (action.File == this.CurrentFile)
            {
                index = this.Files.IndexOf(action.File);
                int lastIndexAfterRemove = this.Files.Count - 2;
                if (index > lastIndexAfterRemove) index = lastIndexAfterRemove;
            }

            this.Files.Remove(action.File);

            if (index != -1)
                this.CurrentFile = this.Files.ElementAt(index);
        }
    }
}