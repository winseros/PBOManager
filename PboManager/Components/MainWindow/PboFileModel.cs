using System.Windows.Input;
using NLog;
using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;

namespace PboManager.Components.MainWindow
{
    public class PboFileModel : ViewModel
    {
        private readonly IMainWindowContext context;
        private readonly ILogger logger;

        public PboFileModel(PboFileModelContext fileModelContext, IMainWindowContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;

            this.Path = fileModelContext.Path;
            this.Name = System.IO.Path.GetFileName(fileModelContext.Path);
            this.Tree = this.CreatePboTree(fileModelContext);
            this.ContextMenu = this.CreateContextMenu(this.Tree);
            this.CommandClose = new Command(this.HandleCommandClose);
        }

        public string Path { get; }

        public string Name { get; }

        public PboTreeModel Tree { get; }

        public ContextMenuModel ContextMenu { get; }

        public ICommand CommandClose { get; }

        private PboTreeModel CreatePboTree(PboFileModelContext fileModelContext)
        {
            var ctx = new  PboTreeModelContext
            {
                FileName = this.Name,
                Pbo = fileModelContext.Pbo
            };
            PboTreeModel model = this.context.GetPboTreeModel(ctx);
            return model;
        }

        private ContextMenuModel CreateContextMenu(PboNodeModel node)
        {
            ContextMenuModel model = this.context.GetContextMenuModel(node);
            return model;
        }

        private void HandleCommandClose(object param)
        {
            this.logger.Debug("Closing the opened file: \"{0}\"", this.Path);
            this.context.GetEventBus().Publish(new FileCloseAction {File = this});
        }
    }
}