using System;
using System.IO;
using System.Reactive.Linq;
using PboManager.Components.MainMenu;
using PboManager.Components.PboTree;
using PboManager.Services.ExceptionService;
using PboTools.Domain;
using PboTools.Service;
using ReactiveUI;
using Util;

namespace PboManager.Components.MainWindow
{
    public class MainWindowModel : ReactiveObject
    {
        private readonly IMainWindowContext context;
        private readonly ObservableAsPropertyHelper<string> title;

        public MainWindowModel(IMainWindowContext context)
        {
            this.context = context;
            this.MainMenu = context.GetMainMenuModel();

            this.MainMenu.WhenAnyValue(p => p.CurrentFileName)
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(Path.GetFileName)
                .ToProperty(this, model => model.Title, out this.title);

            this.MainMenu.WhenAnyValue(menu => menu.CurrentFileName)
                .Where(p => !string.IsNullOrEmpty(p))
                .Subscribe(this.OpenFile);
           
            this.TreeModel = context.GetPboTreeModel();
        }

        public MainMenuModel MainMenu { get; }

        public PboTreeModel TreeModel { get; }

        public string Title => this.title.Value;

        public void OpenFile(string fileName)
        {
            Assert.NotNull(fileName, nameof(fileName));
            try
            {
                IPboArchiverService archiverService = this.context.GetPboArchiverService();
                PboInfo pboInfo = archiverService.GetPboInfo(fileName);
                this.TreeModel.LoadPbo(pboInfo, this.Title);
            }
            catch (Exception ex)
            {
                string message = $"Could not open the file: {fileName}";
                IExceptionService exceptionService = this.context.GetExceptionService();
                exceptionService.ReportException(message, ex);
            }
        }
    }
}