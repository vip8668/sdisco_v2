using Abp.Dependency;
using TepayLink.Sdisco.Configuration;
using TepayLink.Sdisco.Url;
using TepayLink.Sdisco.Web.Url;

namespace TepayLink.Sdisco.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}