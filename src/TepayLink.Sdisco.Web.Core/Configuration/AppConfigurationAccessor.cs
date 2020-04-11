using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TepayLink.Sdisco.Configuration;

namespace TepayLink.Sdisco.Web.Configuration
{
    public class AppConfigurationAccessor: IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public AppConfigurationAccessor(IWebHostEnvironment env)
        {
            Configuration = env.GetAppConfiguration();
        }
    }
}
