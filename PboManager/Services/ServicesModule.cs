using Autofac;
using PboManager.Services.EventBus;
using PboManager.Services.ExceptionService;
using PboManager.Services.FileIconService;
using PboManager.Services.OpenFileService;

namespace PboManager.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventBusImpl>().As<IEventBus>().SingleInstance();
            builder.RegisterType<ExceptionServiceImpl>().As<IExceptionService>().SingleInstance();
            builder.RegisterType<OpenFileServiceImpl>().As<IOpenFileService>().SingleInstance();
            builder.RegisterInstance(new CachingFileIconServiceImpl(new FileIconServiceImpl())).As<IFileIconService>();            
        }
    }
}
