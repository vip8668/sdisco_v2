using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Layout;
using TepayLink.Sdisco.Web.Session;
using TepayLink.Sdisco.Web.Views;

namespace TepayLink.Sdisco.Web.Areas.Admin.Views.Shared.Components.AdminTheme5Footer
{
    public class AdminTheme5FooterViewComponent : SdiscoViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminTheme5FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
