using Autofac;
using NLog;
using PboManager.Services.ExceptionService;
using PboManager.Services.FileIconService;

namespace PboManager.Services
{
    public class ServicesModule : Module
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            logger.Debug("Building the module");

            builder.RegisterType<ExceptionServiceImpl>().As<IExceptionService>().SingleInstance();
            builder.RegisterInstance(new CachingFileIconServiceImpl(new FileIconServiceImpl())).As<IFileIconService>();            
        }
    }
}
