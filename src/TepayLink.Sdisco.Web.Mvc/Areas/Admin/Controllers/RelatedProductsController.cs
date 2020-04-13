using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.RelatedProducts;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RelatedProducts)]
    public class RelatedProductsController : SdiscoControllerBase
    {
        private readonly IRelatedProductsAppService _relatedProductsAppService;

        public RelatedProductsController(IRelatedProductsAppService relatedProductsAppService)
        {
            _relatedProductsAppService = relatedProductsAppService;
        }

        public ActionResult Index()
        {
            var model = new RelatedProductsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RelatedProducts_Create, AppPermissions.Pages_Administration_RelatedProducts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetRelatedProductForEditOutput getRelatedProductForEditOutput;

			if (id.HasValue){
				getRelatedProductForEditOutput = await _relatedProductsAppService.GetRelatedProductForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getRelatedProductForEditOutput = new GetRelatedProductForEditOutput{
					RelatedProduct = new CreateOrEditRelatedProductDto()
				};
			}

            var viewModel = new CreateOrEditRelatedProductModalViewModel()
            {
				RelatedProduct = getRelatedProductForEditOutput.RelatedProduct,
					ProductName = getRelatedProductForEditOutput.ProductName,
					ProductName2 = getRelatedProductForEditOutput.ProductName2
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRelatedProductModal(long id)
        {
			var getRelatedProductForViewDto = await _relatedProductsAppService.GetRelatedProductForView(id);

            var model = new RelatedProductViewModel()
            {
                RelatedProduct = getRelatedProductForViewDto.RelatedProduct
                , ProductName = getRelatedProductForViewDto.ProductName 

                , ProductName2 = getRelatedProductForViewDto.ProductName2 

            };

            return PartialView("_ViewRelatedProductModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RelatedProducts_Create, AppPermissions.Pages_Administration_RelatedProducts_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RelatedProductProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RelatedProductProductLookupTableModal", viewModel);
        }

    }
}