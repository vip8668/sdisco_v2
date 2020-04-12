using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Bookings;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Bookings)]
    public class BookingsController : SdiscoControllerBase
    {
        private readonly IBookingsAppService _bookingsAppService;

        public BookingsController(IBookingsAppService bookingsAppService)
        {
            _bookingsAppService = bookingsAppService;
        }

        public ActionResult Index()
        {
            var model = new BookingsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Bookings_Create, AppPermissions.Pages_Bookings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetBookingForEditOutput getBookingForEditOutput;

			if (id.HasValue){
				getBookingForEditOutput = await _bookingsAppService.GetBookingForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getBookingForEditOutput = new GetBookingForEditOutput{
					Booking = new CreateOrEditBookingDto()
				};
			}

            var viewModel = new CreateOrEditBookingModalViewModel()
            {
				Booking = getBookingForEditOutput.Booking,
					ProductName = getBookingForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBookingModal(int id)
        {
			var getBookingForViewDto = await _bookingsAppService.GetBookingForView(id);

            var model = new BookingViewModel()
            {
                Booking = getBookingForViewDto.Booking
                , ProductName = getBookingForViewDto.ProductName 

            };

            return PartialView("_ViewBookingModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Bookings_Create, AppPermissions.Pages_Bookings_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BookingProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BookingProductLookupTableModal", viewModel);
        }

    }
}