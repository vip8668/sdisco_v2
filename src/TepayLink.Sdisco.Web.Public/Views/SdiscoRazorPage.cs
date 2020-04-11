using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace TepayLink.Sdisco.Web.Public.Views
{
    public abstract class SdiscoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected SdiscoRazorPage()
        {
            LocalizationSourceName = SdiscoConsts.LocalizationSourceName;
        }
    }
}
