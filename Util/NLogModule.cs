using System.Linq;
using Autofac;
using Autofac.Core;
using NLog;
using NLog.Config;

namespace Util
{
    public class NLogModule : Module
    {
        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
            {
                new ResolvedParameter((p, i) => p.ParameterType == typeof(ILogger),
                                      (p, i) => LogManager.GetLogger(t.FullName))
            });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoggingConfiguration config = LogManager.Configuration;
            if (config != null)
                config.DefaultCultureInfo = new NLogCultureInfo();
        }
    }
}
