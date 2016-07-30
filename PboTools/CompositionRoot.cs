using LightInject;
using NLog;
using PboTools.Service;

namespace PboTools
{
    public class CompositionRoot : ICompositionRoot
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Compose(IServiceRegistry serviceRegistry)
        {
            logger.Debug("Building the root");

            serviceRegistry.Register<ILzhService, LzhService>();
            serviceRegistry.Register<IPboArchiverService, PboArchiverService>();
            serviceRegistry.Register<IPboDiskService, PboDiskService>();
            serviceRegistry.Register<IPboInfoService, PboInfoService>();
            serviceRegistry.Register<IPboPackService, PboPackService>();
        }
    }
}
