using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Configuration.Host;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.Timing;
using TepayLink.Sdisco.Timing.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.HostSettings;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsController : SdiscoControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IHostSettingsAppService _hostSettingsAppService;
        private readonly IEditionAppService _editionAppService;
        private readonly ITimingAppService _timingAppService;

        public HostSettingsController(
            IHostSettingsAppService hostSettingsAppService,
            UserManager userManager, 
            IEditionAppService editionAppService, 
            ITimingAppService timingAppService)
        {
            _hostSettingsAppService = hostSettingsAppService;
            _userManager = userManager;
            _editionAppService = editionAppService;
            _timingAppService = timingAppService;
        }

        public async Task<ActionResult> Index()
        {
            var hostSettings = await _hostSettingsAppService.GetAllSettings();
            var editionItems = await _editionAppService.GetEditionComboboxItems(hostSettings.TenantManagement.DefaultEditionId);
            var timezoneItems = await _timingAppService.GetTimezoneComboboxItems(new GetTimezoneComboboxItemsInput
            {
                DefaultTimezoneScope = SettingScopes.Application,
                SelectedTimezoneId = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone)
            });

            var user = await _userManager.GetUserAsync(AbpSession.ToUserIdentifier());

            ViewBag.CurrentUserEmail = user.EmailAddress;

            var model = new HostSettingsViewModel
            {
                Settings = hostSettings,
                EditionItems = editionItems,
                TimezoneItems = timezoneItems
            };

            return View(model);
        }
    }
}