using Autofac;
using Autofac.Extras.AggregateService;

namespace PboManager.Components.MainMenu
{
    public class MainMenuModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterType<MainMenuModel>().ExternallyOwned();
            builder.RegisterAggregateService<IMainMenuContext>();
        }
    }
}
