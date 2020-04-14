using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductSchedules;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductSchedules)]
    public class ProductSchedulesController : SdiscoControllerBase
    {
        private readonly IProductSchedulesAppService _productSchedulesAppService;

        public ProductSchedulesController(IProductSchedulesAppService productSchedulesAppService)
        {
            _productSchedulesAppService = productSchedulesAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductSchedulesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductSchedules_Create, AppPermissions.Pages_Administration_ProductSchedules_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductScheduleForEditOutput getProductScheduleForEditOutput;

			if (id.HasValue){
				getProductScheduleForEditOutput = await _productSchedulesAppService.GetProductScheduleForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductScheduleForEditOutput = new GetProductScheduleForEditOutput{
					ProductSchedule = new CreateOrEditProductScheduleDto()
				};
				getProductScheduleForEditOutput.ProductSchedule.StartDate = DateTime.Now;
				getProductScheduleForEditOutput.ProductSchedule.EndDate = DateTime.Now;
			}

            var viewModel = new CreateOrEditProductScheduleModalViewModel()
            {
				ProductSchedule = getProductScheduleForEditOutput.ProductSchedule,
					ProductName = getProductScheduleForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductScheduleModal(long id)
        {
			var getProductScheduleForViewDto = await _productSchedulesAppService.GetProductScheduleForView(id);

            var model = new ProductScheduleViewModel()
            {
                ProductSchedule = getProductScheduleForViewDto.ProductSchedule
                , ProductName = getProductScheduleForViewDto.ProductName 

            };

            return PartialView("_ViewProductScheduleModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductSchedules_Create, AppPermissions.Pages_Administration_ProductSchedules_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductScheduleProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductScheduleProductLookupTableModal", viewModel);
        }

    }
}