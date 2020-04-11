using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using TepayLink.Sdisco.Configuration;

namespace TepayLink.Sdisco.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(SdiscoTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
