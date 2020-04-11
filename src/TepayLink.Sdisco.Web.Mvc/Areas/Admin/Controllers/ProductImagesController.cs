using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductImages;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_ProductImages)]
    public class ProductImagesController : SdiscoControllerBase
    {
        private readonly IProductImagesAppService _productImagesAppService;

        public ProductImagesController(IProductImagesAppService productImagesAppService)
        {
            _productImagesAppService = productImagesAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductImagesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_ProductImages_Create, AppPermissions.Pages_ProductImages_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductImageForEditOutput getProductImageForEditOutput;

			if (id.HasValue){
				getProductImageForEditOutput = await _productImagesAppService.GetProductImageForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductImageForEditOutput = new GetProductImageForEditOutput{
					ProductImage = new CreateOrEditProductImageDto()
				};
			}

            var viewModel = new CreateOrEditProductImageModalViewModel()
            {
				ProductImage = getProductImageForEditOutput.ProductImage,
					ProductName = getProductImageForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductImageModal(long id)
        {
			var getProductImageForViewDto = await _productImagesAppService.GetProductImageForView(id);

            var model = new ProductImageViewModel()
            {
                ProductImage = getProductImageForViewDto.ProductImage
                , ProductName = getProductImageForViewDto.ProductName 

            };

            return PartialView("_ViewProductImageModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ProductImages_Create, AppPermissions.Pages_ProductImages_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductImageProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductImageProductLookupTableModal", viewModel);
        }

    }
}