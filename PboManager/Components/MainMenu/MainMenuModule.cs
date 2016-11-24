using Autofac;
using Autofac.Extras.AggregateService;
using NLog;

namespace PboManager.Components.MainMenu
{
    public class MainMenuModule : Module
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            logger.Debug("Building the module");

            builder.RegisterType<OpenPboService>().As<IOpenPboService>().SingleInstance();            
            builder.RegisterType<MainMenuModel>().SingleInstance();
            builder.RegisterAggregateService<IMainMenuContext>();
        }
    }
}
