using System;
using System.Windows;
using System.Windows.Input;
using NLog;
using PboManager.Components.MainWindow;
using PboManager.Services.EventBus;
using PboManager.Services.OpenFileService;
using PboTools.Domain;
using PboTools.Service;

namespace PboManager.Components.MainMenu
{
    public class MainMenuModel : ViewModel
    {
        private readonly IMainMenuContext context;
        private readonly ILogger logger;

        private PboFileModel currentFile;

        [Obsolete("For XAML designer")]
        public MainMenuModel()
        {
        }

        public MainMenuModel(IMainMenuContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;

            this.CommandOpenFile = new Command(this.InvokeCommandOpenFileSafe);
            this.CommandCloseFile = new Command(this.InvokeCommandCloseFile, this.CanCommandCloseFile);
            this.CommandExit = new Command(o => Application.Current.Shutdown());

            this.context.GetEventBus().Subscribe<CurrentFileChangedAction>(this.HandleCurrentFileChangedAction);
        }

        public ICommand CommandOpenFile { get; }

        public ICommand CommandCloseFile { get; }

        public ICommand CommandExit { get; }

        private void InvokeCommandOpenFileSafe(object param)
        {
            try
            {
                this.InvokeCommandOpenFile(param);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                this.context.GetExceptionService().ReportException("An error occurred opening the file", ex);
            }
        }

        private void InvokeCommandOpenFile(object param)
        {
            this.logger.Debug("Opening a pbo file");

            IOpenFileService openFileService = this.context.GetOpenFileService();
            string path = openFileService.OpenFile();
            if (!string.IsNullOrEmpty(path))
            {
                this.logger.Debug("User selected a file: \"{0}\"", path);
                IPboArchiverService archiverService = this.context.GetArchiverService();
                PboInfo pboInfo = archiverService.GetPboInfo(path);
                var action = new FileOpenedAction {Path = path, Pbo = pboInfo};

                IEventBus eventBus = this.context.GetEventBus();
                eventBus.Publish(action);
            }
            else
            {
                this.logger.Debug("User cancelled a file open dialog");
            }
        }

        private void InvokeCommandCloseFile(object param)
        {
            var action = new FileCloseAction {File = this.currentFile};
            this.context.GetEventBus().Publish(action);
        }

        private bool CanCommandCloseFile(object param)
        {
            return this.currentFile != null;
        }

        private void HandleCurrentFileChangedAction(CurrentFileChangedAction action)
        {
            this.currentFile = action.File;
            ((Command)this.CommandCloseFile).RaiseCanExecuteChanged();
        }
    }
}