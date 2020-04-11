using Abp.AspNetCore.Mvc.Views;

namespace TepayLink.Sdisco.Web.Views
{
    public abstract class SdiscoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected SdiscoRazorPage()
        {
            LocalizationSourceName = SdiscoConsts.LocalizationSourceName;
        }
    }
}
