using Autofac;
using Autofac.Extras.AggregateService;

namespace PboManager.Components.MainWindow
{
    public class MainWindowModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowModel>().ExternallyOwned();
            builder.RegisterType<PboFileModel>().ExternallyOwned();
            builder.RegisterAggregateService<IMainWindowContext>();
        }
    }
}
