using Autofac;
using Autofac.Extras.AggregateService;
using NLog;

namespace PboManager.Components.MainWindow
{
    public class MainWindowModule : Module
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            logger.Debug("Building the module");

            builder.RegisterType<MainWindowModel>().SingleInstance();
            builder.RegisterAggregateService<IMainWindowContext>();
        }
    }
}
