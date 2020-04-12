using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BookingClaims;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Booking;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingClaims)]
    public class BookingClaimsController : SdiscoControllerBase
    {
        private readonly IBookingClaimsAppService _bookingClaimsAppService;

        public BookingClaimsController(IBookingClaimsAppService bookingClaimsAppService)
        {
            _bookingClaimsAppService = bookingClaimsAppService;
        }

        public ActionResult Index()
        {
            var model = new BookingClaimsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingClaims_Create, AppPermissions.Pages_Administration_BookingClaims_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBookingClaimForEditOutput getBookingClaimForEditOutput;

			if (id.HasValue){
				getBookingClaimForEditOutput = await _bookingClaimsAppService.GetBookingClaimForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBookingClaimForEditOutput = new GetBookingClaimForEditOutput{
					BookingClaim = new CreateOrEditBookingClaimDto()
				};
			}

            var viewModel = new CreateOrEditBookingClaimModalViewModel()
            {
				BookingClaim = getBookingClaimForEditOutput.BookingClaim,
					ClaimReasonTitle = getBookingClaimForEditOutput.ClaimReasonTitle
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBookingClaimModal(long id)
        {
			var getBookingClaimForViewDto = await _bookingClaimsAppService.GetBookingClaimForView(id);

            var model = new BookingClaimViewModel()
            {
                BookingClaim = getBookingClaimForViewDto.BookingClaim
                , ClaimReasonTitle = getBookingClaimForViewDto.ClaimReasonTitle 

            };

            return PartialView("_ViewBookingClaimModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingClaims_Create, AppPermissions.Pages_Administration_BookingClaims_Edit)]
        public PartialViewResult ClaimReasonLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BookingClaimClaimReasonLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BookingClaimClaimReasonLookupTableModal", viewModel);
        }

    }
}