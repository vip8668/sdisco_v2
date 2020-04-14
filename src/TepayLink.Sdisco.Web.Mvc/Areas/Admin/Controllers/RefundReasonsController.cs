using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.RefundReasons;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RefundReasons)]
    public class RefundReasonsController : SdiscoControllerBase
    {
        private readonly IRefundReasonsAppService _refundReasonsAppService;

        public RefundReasonsController(IRefundReasonsAppService refundReasonsAppService)
        {
            _refundReasonsAppService = refundReasonsAppService;
        }

        public ActionResult Index()
        {
            var model = new RefundReasonsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RefundReasons_Create, AppPermissions.Pages_Administration_RefundReasons_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetRefundReasonForEditOutput getRefundReasonForEditOutput;

			if (id.HasValue){
				getRefundReasonForEditOutput = await _refundReasonsAppService.GetRefundReasonForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getRefundReasonForEditOutput = new GetRefundReasonForEditOutput{
					RefundReason = new CreateOrEditRefundReasonDto()
				};
			}

            var viewModel = new CreateOrEditRefundReasonModalViewModel()
            {
				RefundReason = getRefundReasonForEditOutput.RefundReason
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRefundReasonModal(int id)
        {
			var getRefundReasonForViewDto = await _refundReasonsAppService.GetRefundReasonForView(id);

            var model = new RefundReasonViewModel()
            {
                RefundReason = getRefundReasonForViewDto.RefundReason
            };

            return PartialView("_ViewRefundReasonModal", model);
        }


    }
}