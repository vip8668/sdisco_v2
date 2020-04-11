using Abp.AspNetCore.Mvc.ViewComponents;

namespace TepayLink.Sdisco.Web.Views
{
    public abstract class SdiscoViewComponent : AbpViewComponent
    {
        protected SdiscoViewComponent()
        {
            LocalizationSourceName = SdiscoConsts.LocalizationSourceName;
        }
    }
}