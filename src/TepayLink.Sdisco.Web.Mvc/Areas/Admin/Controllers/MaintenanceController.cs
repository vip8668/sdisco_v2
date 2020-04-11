using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Caching;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Maintenance;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
    public class MaintenanceController : SdiscoControllerBase
    {
        private readonly ICachingAppService _cachingAppService;

        public MaintenanceController(ICachingAppService cachingAppService)
        {
            _cachingAppService = cachingAppService;
        }

        public ActionResult Index()
        {
            var model = new MaintenanceViewModel
            {
                Caches = _cachingAppService.GetAllCaches().Items
            };

            return View(model);
        }
    }
}