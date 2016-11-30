using Autofac;
using NLog;
using PboTools.Service;

namespace PboTools
{
    public class AutofacModule : Module
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            logger.Debug("Building the module");

            builder.RegisterType<LzhService>().As<ILzhService>().SingleInstance();
            builder.RegisterType<PboArchiverService>().As<IPboArchiverService>().SingleInstance();
            builder.RegisterType<PboDiskService>().As<IPboDiskService>().SingleInstance();
            builder.RegisterType<PboInfoService>().As<IPboInfoService>().SingleInstance();
            builder.RegisterType<PboPackService>().As<IPboPackService>().SingleInstance();
            builder.RegisterType<TimestampService>().As<ITimestampService>().SingleInstance();
        }
    }
}
