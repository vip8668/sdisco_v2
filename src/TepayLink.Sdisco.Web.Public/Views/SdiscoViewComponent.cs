using Abp.AspNetCore.Mvc.ViewComponents;

namespace TepayLink.Sdisco.Web.Public.Views
{
    public abstract class SdiscoViewComponent : AbpViewComponent
    {
        protected SdiscoViewComponent()
        {
            LocalizationSourceName = SdiscoConsts.LocalizationSourceName;
        }
    }
}