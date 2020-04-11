using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Configuration;
using TepayLink.Sdisco.Web.Areas.Admin.Models.UiCustomization;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize]
    public class UiCustomizationController : SdiscoControllerBase
    {
        private readonly IUiCustomizationSettingsAppService _uiCustomizationAppService;

        public UiCustomizationController(IUiCustomizationSettingsAppService uiCustomizationAppService)
        {
            _uiCustomizationAppService = uiCustomizationAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new UiCustomizationViewModel
            {
                Theme = await SettingManager.GetSettingValueAsync(AppSettings.UiManagement.Theme),
                Settings = await _uiCustomizationAppService.GetUiManagementSettings(),
                HasUiCustomizationPagePermission = await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_Administration_UiCustomization)
            };

            return View(model);
        }
    }
}