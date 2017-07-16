using Autofac;
using Autofac.Extras.AggregateService;

namespace PboManager.Components.PboTree
{
    public class PboTreeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PboTreeModel>().ExternallyOwned();
            builder.RegisterType<PboNodeModel>().ExternallyOwned();
            builder.RegisterAggregateService<IPboTreeContext>();
        }
    }
}
