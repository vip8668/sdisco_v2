using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Orders;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Orders)]
    public class OrdersController : SdiscoControllerBase
    {
        private readonly IOrdersAppService _ordersAppService;

        public OrdersController(IOrdersAppService ordersAppService)
        {
            _ordersAppService = ordersAppService;
        }

        public ActionResult Index()
        {
            var model = new OrdersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Orders_Create, AppPermissions.Pages_Administration_Orders_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetOrderForEditOutput getOrderForEditOutput;

			if (id.HasValue){
				getOrderForEditOutput = await _ordersAppService.GetOrderForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getOrderForEditOutput = new GetOrderForEditOutput{
					Order = new CreateOrEditOrderDto()
				};
			}

            var viewModel = new CreateOrEditOrderModalViewModel()
            {
				Order = getOrderForEditOutput.Order
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewOrderModal(long id)
        {
			var getOrderForViewDto = await _ordersAppService.GetOrderForView(id);

            var model = new OrderViewModel()
            {
                Order = getOrderForViewDto.Order
            };

            return PartialView("_ViewOrderModal", model);
        }


    }
}