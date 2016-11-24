using System.Windows;
using Autofac;
using PboManager.Components.MainWindow;

namespace PboManager
{
    public partial class App : Application
    {
        private IContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var builder = new ContainerBuilder();
            builder.RegisterModule<PboTools.AutofacModule>();
            builder.RegisterAssemblyModules(typeof(App).Assembly);
            this.container = builder.Build();
            
            var mainWindowModel = this.container.Resolve<MainWindowModel>();
            new MainWindow(mainWindowModel).Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.container.Dispose();
        }
    }
}