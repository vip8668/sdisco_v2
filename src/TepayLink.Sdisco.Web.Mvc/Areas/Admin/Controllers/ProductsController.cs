using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Products;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products)]
    public class ProductsController : SdiscoControllerBase
    {
        private readonly IProductsAppService _productsAppService;

        public ProductsController(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products_Create, AppPermissions.Pages_Administration_Products_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductForEditOutput getProductForEditOutput;

			if (id.HasValue){
				getProductForEditOutput = await _productsAppService.GetProductForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductForEditOutput = new GetProductForEditOutput{
					Product = new CreateOrEditProductDto()
				};
			}

            var viewModel = new CreateOrEditProductModalViewModel()
            {
				Product = getProductForEditOutput.Product,
					CategoryName = getProductForEditOutput.CategoryName,
					UserName = getProductForEditOutput.UserName,
					PlaceName = getProductForEditOutput.PlaceName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductModal(long id)
        {
			var getProductForViewDto = await _productsAppService.GetProductForView(id);

            var model = new ProductViewModel()
            {
                Product = getProductForViewDto.Product
                , CategoryName = getProductForViewDto.CategoryName 

                , UserName = getProductForViewDto.UserName 

                , PlaceName = getProductForViewDto.PlaceName 

            };

            return PartialView("_ViewProductModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products_Create, AppPermissions.Pages_Administration_Products_Edit)]
        public PartialViewResult CategoryLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ProductCategoryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductCategoryLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products_Create, AppPermissions.Pages_Administration_Products_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Products_Create, AppPermissions.Pages_Administration_Products_Edit)]
        public PartialViewResult PlaceLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductPlaceLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductPlaceLookupTableModal", viewModel);
        }

    }
}