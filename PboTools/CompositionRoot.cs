using LightInject;
using PboTools.Service;

namespace PboTools
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ILzhService, LzhService>();
            serviceRegistry.Register<IPboArchiverService, PboArchiverService>();
            serviceRegistry.Register<IPboDiskService, PboDiskService>();
            serviceRegistry.Register<IPboInfoService, PboInfoService>();
            serviceRegistry.Register<IPboPackService, PboPackService>();
        }
    }
}
