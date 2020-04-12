using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ClientSettings;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Client;
using TepayLink.Sdisco.Client.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ClientSettings)]
    public class ClientSettingsController : SdiscoControllerBase
    {
        private readonly IClientSettingsAppService _clientSettingsAppService;

        public ClientSettingsController(IClientSettingsAppService clientSettingsAppService)
        {
            _clientSettingsAppService = clientSettingsAppService;
        }

        public ActionResult Index()
        {
            var model = new ClientSettingsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ClientSettings_Create, AppPermissions.Pages_Administration_ClientSettings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetClientSettingForEditOutput getClientSettingForEditOutput;

			if (id.HasValue){
				getClientSettingForEditOutput = await _clientSettingsAppService.GetClientSettingForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getClientSettingForEditOutput = new GetClientSettingForEditOutput{
					ClientSetting = new CreateOrEditClientSettingDto()
				};
			}

            var viewModel = new CreateOrEditClientSettingModalViewModel()
            {
				ClientSetting = getClientSettingForEditOutput.ClientSetting
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewClientSettingModal(long id)
        {
			var getClientSettingForViewDto = await _clientSettingsAppService.GetClientSettingForView(id);

            var model = new ClientSettingViewModel()
            {
                ClientSetting = getClientSettingForViewDto.ClientSetting
            };

            return PartialView("_ViewClientSettingModal", model);
        }


    }
}