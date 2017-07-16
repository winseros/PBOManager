using System.Windows.Input;
using NLog;
using PboManager.Components.PboTree;
using PboManager.Services.EventBus;

namespace PboManager.Components.MainWindow
{
    public class PboFileModel : ViewModel
    {
        private string path;
        private string name;

        private readonly IEventBus eventBus;
        private readonly ILogger logger;

        public PboFileModel(IEventBus eventBus, ILogger logger)
        {
            this.eventBus = eventBus;
            this.logger = logger;

            this.CommandClose = new Command(this.HandleCommandClose);
        }

        public string Path
        {
            get => this.path;
            set
            {
                this.path = value;
                this.Name = System.IO.Path.GetFileName(value);
            }
        }

        public string Name
        {
            get => this.name;
            private set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public PboTreeModel Tree { get; set; }

        public ICommand CommandClose { get; }

        private void HandleCommandClose(object param)
        {
            this.logger.Debug("Closing the opened file: \"{0}\"", this.path);
            this.eventBus.Publish(new FileCloseAction {File = this});
        }
    }
}