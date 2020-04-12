using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.PartnerShips;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_PartnerShips)]
    public class PartnerShipsController : SdiscoControllerBase
    {
        private readonly IPartnerShipsAppService _partnerShipsAppService;

        public PartnerShipsController(IPartnerShipsAppService partnerShipsAppService)
        {
            _partnerShipsAppService = partnerShipsAppService;
        }

        public ActionResult Index()
        {
            var model = new PartnerShipsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_PartnerShips_Create, AppPermissions.Pages_Administration_PartnerShips_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetPartnerShipForEditOutput getPartnerShipForEditOutput;

			if (id.HasValue){
				getPartnerShipForEditOutput = await _partnerShipsAppService.GetPartnerShipForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getPartnerShipForEditOutput = new GetPartnerShipForEditOutput{
					PartnerShip = new CreateOrEditPartnerShipDto()
				};
			}

            var viewModel = new CreateOrEditPartnerShipModalViewModel()
            {
				PartnerShip = getPartnerShipForEditOutput.PartnerShip
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewPartnerShipModal(int id)
        {
			var getPartnerShipForViewDto = await _partnerShipsAppService.GetPartnerShipForView(id);

            var model = new PartnerShipViewModel()
            {
                PartnerShip = getPartnerShipForViewDto.PartnerShip
            };

            return PartialView("_ViewPartnerShipModal", model);
        }


    }
}