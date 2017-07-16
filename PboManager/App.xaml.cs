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
                        
            var mainWindow = new MainWindow();
            mainWindow.DataContext = this.container.Resolve<MainWindowModel>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.container.Dispose();
        }
    }
}