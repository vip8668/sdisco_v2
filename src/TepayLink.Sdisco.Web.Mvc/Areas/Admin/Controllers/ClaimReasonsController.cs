using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ClaimReasons;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;

using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using TepayLink.Sdisco.Bookings;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ClaimReasons)]
    public class ClaimReasonsController : SdiscoControllerBase
    {
        private readonly IClaimReasonsAppService _claimReasonsAppService;

        public ClaimReasonsController(IClaimReasonsAppService claimReasonsAppService)
        {
            _claimReasonsAppService = claimReasonsAppService;
        }

        public ActionResult Index()
        {
            var model = new ClaimReasonsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ClaimReasons_Create, AppPermissions.Pages_Administration_ClaimReasons_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetClaimReasonForEditOutput getClaimReasonForEditOutput;

			if (id.HasValue){
				getClaimReasonForEditOutput = await _claimReasonsAppService.GetClaimReasonForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getClaimReasonForEditOutput = new GetClaimReasonForEditOutput{
					ClaimReason = new CreateOrEditClaimReasonDto()
				};
			}

            var viewModel = new CreateOrEditClaimReasonModalViewModel()
            {
				ClaimReason = getClaimReasonForEditOutput.ClaimReason
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewClaimReasonModal(int id)
        {
			var getClaimReasonForViewDto = await _claimReasonsAppService.GetClaimReasonForView(id);

            var model = new ClaimReasonViewModel()
            {
                ClaimReason = getClaimReasonForViewDto.ClaimReason
            };

            return PartialView("_ViewClaimReasonModal", model);
        }


    }
}