using System;
using System.Windows.Input;
using NLog;
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

        [Obsolete("For XAML designer")]
        public MainMenuModel()
        {
        }

        public MainMenuModel(IMainMenuContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;

            this.CommandNewFile = new Command(this.InvokeCommandNewFileSafe);
            this.CommandOpenFile = new Command(this.InvokeCommandOpenFile);
            this.CommandCloseFile = new Command(this.InvokeCommandCloseFile);
        }

        public ICommand CommandNewFile { get; }

        public ICommand CommandOpenFile { get; }

        public ICommand CommandCloseFile { get; }

        private void InvokeCommandNewFileSafe(object param)
        {
            try
            {
                this.InvokeCommandNewFile(param);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                this.context.GetExceptionService().ReportException("An error occurred opening the file", ex);
            }
        }

        private void InvokeCommandNewFile(object param)
        {
            this.logger.Debug("Opening a pbo file");

            IOpenFileService openFileService = this.context.GetOpenFileService();
            string path = openFileService.OpenFile();
            if (!string.IsNullOrEmpty(path))
            {
                this.logger.Debug("User selected a file: \"{0}\"", path);
                IPboArchiverService archiverService = this.context.GetArchiverService();
                PboInfo pboInfo = archiverService.GetPboInfo(path);
                var action = new FileOpenedAction{Path = path, Pbo = pboInfo};

                IEventBus eventBus = this.context.GetEventBus();
                eventBus.Publish(action);
            }
            else
            {
                this.logger.Debug("User cancelled a file open dialog");
            }
        }

        private void InvokeCommandOpenFile(object param)
        {
            
        }

        private void InvokeCommandCloseFile(object param)
        {
            
        }
    }
}