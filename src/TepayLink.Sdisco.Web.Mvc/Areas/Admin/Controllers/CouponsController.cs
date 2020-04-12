using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Coupons;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Coupons)]
    public class CouponsController : SdiscoControllerBase
    {
        private readonly ICouponsAppService _couponsAppService;

        public CouponsController(ICouponsAppService couponsAppService)
        {
            _couponsAppService = couponsAppService;
        }

        public ActionResult Index()
        {
            var model = new CouponsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Coupons_Create, AppPermissions.Pages_Administration_Coupons_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetCouponForEditOutput getCouponForEditOutput;

			if (id.HasValue){
				getCouponForEditOutput = await _couponsAppService.GetCouponForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getCouponForEditOutput = new GetCouponForEditOutput{
					Coupon = new CreateOrEditCouponDto()
				};
			}

            var viewModel = new CreateOrEditCouponModalViewModel()
            {
				Coupon = getCouponForEditOutput.Coupon
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCouponModal(long id)
        {
			var getCouponForViewDto = await _couponsAppService.GetCouponForView(id);

            var model = new CouponViewModel()
            {
                Coupon = getCouponForViewDto.Coupon
            };

            return PartialView("_ViewCouponModal", model);
        }


    }
}