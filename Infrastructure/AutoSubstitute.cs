using Autofac;
using Util;

namespace Infrastructure
{
    public class AutoSubstitute : AutofacContrib.NSubstitute.AutoSubstitute
    {
        public AutoSubstitute() : base(builder => { builder.RegisterModule<NLogModule>(); })
        {
        }
    }
}