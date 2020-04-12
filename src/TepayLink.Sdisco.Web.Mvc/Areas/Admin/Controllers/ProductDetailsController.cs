using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductDetails;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetails)]
    public class ProductDetailsController : SdiscoControllerBase
    {
        private readonly IProductDetailsAppService _productDetailsAppService;

        public ProductDetailsController(IProductDetailsAppService productDetailsAppService)
        {
            _productDetailsAppService = productDetailsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductDetailsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetails_Create, AppPermissions.Pages_Administration_ProductDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductDetailForEditOutput getProductDetailForEditOutput;

			if (id.HasValue){
				getProductDetailForEditOutput = await _productDetailsAppService.GetProductDetailForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductDetailForEditOutput = new GetProductDetailForEditOutput{
					ProductDetail = new CreateOrEditProductDetailDto()
				};
			}

            var viewModel = new CreateOrEditProductDetailModalViewModel()
            {
				ProductDetail = getProductDetailForEditOutput.ProductDetail,
					ProductName = getProductDetailForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductDetailModal(long id)
        {
			var getProductDetailForViewDto = await _productDetailsAppService.GetProductDetailForView(id);

            var model = new ProductDetailViewModel()
            {
                ProductDetail = getProductDetailForViewDto.ProductDetail
                , ProductName = getProductDetailForViewDto.ProductName 

            };

            return PartialView("_ViewProductDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetails_Create, AppPermissions.Pages_Administration_ProductDetails_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductDetailProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductDetailProductLookupTableModal", viewModel);
        }

    }
}