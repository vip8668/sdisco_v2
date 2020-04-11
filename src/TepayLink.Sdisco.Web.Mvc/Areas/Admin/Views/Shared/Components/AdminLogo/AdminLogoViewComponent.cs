using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Layout;
using TepayLink.Sdisco.Web.Session;
using TepayLink.Sdisco.Web.Views;

namespace TepayLink.Sdisco.Web.Areas.Admin.Views.Shared.Components.AdminLogo
{
    public class AdminLogoViewComponent : SdiscoViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AdminLogoViewComponent(
            IPerRequestSessionCache sessionCache
        )
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string logoSkin = null, string logoClass = "")
        {
            var headerModel = new LogoViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                LogoSkinOverride = logoSkin,
                LogoClassOverride = logoClass
            };

            return View(headerModel);
        }
    }
}
