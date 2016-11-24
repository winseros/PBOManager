using System.Windows;
using LightInject;
using PboManager.Components;
using PboManager.Components.MainWindow;

namespace PboManager
{
    public partial class App : Application
    {
        private readonly ServiceContainer compositionRoot = new ServiceContainer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            this.compositionRoot.EnableAutoFactories();            
            this.compositionRoot.SetDefaultLifetime<PerContainerLifetime>();
            this.compositionRoot.RegisterFrom<PboTools.CompositionRoot>();
            this.compositionRoot.RegisterFrom<CompositionRoot>();

            var mainWindowModel = this.compositionRoot.GetInstance<MainWindowModel>();
            new MainWindow(mainWindowModel).Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.compositionRoot.Dispose();
        }
    }
}