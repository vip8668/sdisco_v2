using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.DashboardCustomization;
using TepayLink.Sdisco.Web.DashboardCustomization;
using System.Threading.Tasks;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class HostDashboardController : CustomizableDashboardControllerBase
    {
        public HostDashboardController(
            DashboardViewConfiguration dashboardViewConfiguration,
            IDashboardCustomizationAppService dashboardCustomizationAppService)
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(SdiscoDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard);
        }
    }
}