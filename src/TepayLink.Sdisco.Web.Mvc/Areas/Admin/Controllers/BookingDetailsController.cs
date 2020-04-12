using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BookingDetails;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingDetails)]
    public class BookingDetailsController : SdiscoControllerBase
    {
        private readonly IBookingDetailsAppService _bookingDetailsAppService;

        public BookingDetailsController(IBookingDetailsAppService bookingDetailsAppService)
        {
            _bookingDetailsAppService = bookingDetailsAppService;
        }

        public ActionResult Index()
        {
            var model = new BookingDetailsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingDetails_Create, AppPermissions.Pages_Administration_BookingDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBookingDetailForEditOutput getBookingDetailForEditOutput;

			if (id.HasValue){
				getBookingDetailForEditOutput = await _bookingDetailsAppService.GetBookingDetailForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBookingDetailForEditOutput = new GetBookingDetailForEditOutput{
					BookingDetail = new CreateOrEditBookingDetailDto()
				};
			}

            var viewModel = new CreateOrEditBookingDetailModalViewModel()
            {
				BookingDetail = getBookingDetailForEditOutput.BookingDetail,
					ProductName = getBookingDetailForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBookingDetailModal(long id)
        {
			var getBookingDetailForViewDto = await _bookingDetailsAppService.GetBookingDetailForView(id);

            var model = new BookingDetailViewModel()
            {
                BookingDetail = getBookingDetailForViewDto.BookingDetail
                , ProductName = getBookingDetailForViewDto.ProductName 

            };

            return PartialView("_ViewBookingDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BookingDetails_Create, AppPermissions.Pages_Administration_BookingDetails_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BookingDetailProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BookingDetailProductLookupTableModal", viewModel);
        }

    }
}