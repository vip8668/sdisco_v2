using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BookingRefunds;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingRefunds)]
    public class BookingRefundsController : SdiscoControllerBase
    {
        private readonly IBookingRefundsAppService _bookingRefundsAppService;

        public BookingRefundsController(IBookingRefundsAppService bookingRefundsAppService)
        {
            _bookingRefundsAppService = bookingRefundsAppService;
        }

        public ActionResult Index()
        {
            var model = new BookingRefundsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingRefunds_Create, AppPermissions.Pages_Administration_BookingRefunds_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBookingRefundForEditOutput getBookingRefundForEditOutput;

			if (id.HasValue){
				getBookingRefundForEditOutput = await _bookingRefundsAppService.GetBookingRefundForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBookingRefundForEditOutput = new GetBookingRefundForEditOutput{
					BookingRefund = new CreateOrEditBookingRefundDto()
				};
			}

            var viewModel = new CreateOrEditBookingRefundModalViewModel()
            {
				BookingRefund = getBookingRefundForEditOutput.BookingRefund
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBookingRefundModal(long id)
        {
			var getBookingRefundForViewDto = await _bookingRefundsAppService.GetBookingRefundForView(id);

            var model = new BookingRefundViewModel()
            {
                BookingRefund = getBookingRefundForViewDto.BookingRefund
            };

            return PartialView("_ViewBookingRefundModal", model);
        }


    }
}