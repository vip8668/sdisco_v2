using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Editions;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Web.Session;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)]
    public class SubscriptionManagementController : SdiscoControllerBase
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public SubscriptionManagementController(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<ActionResult> Index()
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            var model = new SubscriptionDashboardViewModel
            {
                LoginInformations = loginInfo
            };

            return View(model);
        }
    }
}