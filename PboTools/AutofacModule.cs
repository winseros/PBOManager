using Autofac;
using PboTools.Service;

namespace PboTools
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LzhService>().As<ILzhService>().SingleInstance();
            builder.RegisterType<PboArchiverService>().As<IPboArchiverService>().SingleInstance();
            builder.RegisterType<PboDiskService>().As<IPboDiskService>().SingleInstance();
            builder.RegisterType<PboInfoService>().As<IPboInfoService>().SingleInstance();
            builder.RegisterType<PboPackService>().As<IPboPackService>().SingleInstance();
            builder.RegisterType<TimestampService>().As<ITimestampService>().SingleInstance();
        }
    }
}
