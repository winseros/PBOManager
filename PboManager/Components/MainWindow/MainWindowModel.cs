using System;
using System.Collections.ObjectModel;
using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.EventBus;

namespace PboManager.Components.MainWindow
{
    public class MainWindowModel : ViewModel
    {
        private readonly IMainWindowContext context;
        private readonly ObservableCollection<PboFileModel> files = new ObservableCollection<PboFileModel>();

        [Obsolete("For XAML designer")]
        public MainWindowModel()
        {
        }

        public MainWindowModel(IMainWindowContext context)
        {
            this.context = context;
            this.MainMenu = context.GetMainMenuModel();

            IEventBus eventBus = this.context.GetEventBus();
            eventBus.Subscribe<FileOpenedAction>(this.HandleFileOpenedAction);
            eventBus.Subscribe<FileCloseAction>(this.HandleFileCloseAction);
        }

        public MainMenuModel MainMenu { get; }

        public ObservableCollection<PboFileModel> Files => this.files;

        private void HandleFileOpenedAction(FileOpenedAction action)
        {
            PboTreeModel tree = this.context.GetPboTreeModel(action.Pbo);
            PboFileModel file = this.context.GetPboFileModel();
            file.Path = action.Path;
            file.Tree = tree;
            this.files.Add(file);
        }

        private void HandleFileCloseAction(FileCloseAction action)
        {
            this.files.Remove(action.File);
        }
    }
}