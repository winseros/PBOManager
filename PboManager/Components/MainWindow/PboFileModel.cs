using System.Windows.Input;
using NLog;
using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.EventBus;

namespace PboManager.Components.MainWindow
{
    public class PboFileModel : ViewModel
    {
        private readonly IEventBus eventBus;
        private readonly ILogger logger;

        public PboFileModel(PboFileModelContext fileModelContext, IEventBus eventBus, ILogger logger)
        {
            this.eventBus = eventBus;
            this.logger = logger;

            this.Path = fileModelContext.Path;
            this.Name = System.IO.Path.GetFileName(fileModelContext.Path);
            this.Tree = fileModelContext.Tree;

            this.CommandClose = new Command(this.HandleCommandClose);
        }

        public string Path { get; }

        public string Name { get; }

        public PboTreeModel Tree { get; }

        public ICommand CommandClose { get; }

        private void HandleCommandClose(object param)
        {
            this.logger.Debug("Closing the opened file: \"{0}\"", this.Path);
            this.eventBus.Publish(new FileCloseAction {File = this});
        }
    }
}