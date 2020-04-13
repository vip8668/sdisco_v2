using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.SimilarProducts;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_SimilarProducts)]
    public class SimilarProductsController : SdiscoControllerBase
    {
        private readonly ISimilarProductsAppService _similarProductsAppService;

        public SimilarProductsController(ISimilarProductsAppService similarProductsAppService)
        {
            _similarProductsAppService = similarProductsAppService;
        }

        public ActionResult Index()
        {
            var model = new SimilarProductsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_SimilarProducts_Create, AppPermissions.Pages_Administration_SimilarProducts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetSimilarProductForEditOutput getSimilarProductForEditOutput;

			if (id.HasValue){
				getSimilarProductForEditOutput = await _similarProductsAppService.GetSimilarProductForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getSimilarProductForEditOutput = new GetSimilarProductForEditOutput{
					SimilarProduct = new CreateOrEditSimilarProductDto()
				};
			}

            var viewModel = new CreateOrEditSimilarProductModalViewModel()
            {
				SimilarProduct = getSimilarProductForEditOutput.SimilarProduct,
					ProductName = getSimilarProductForEditOutput.ProductName,
					ProductName2 = getSimilarProductForEditOutput.ProductName2
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSimilarProductModal(long id)
        {
			var getSimilarProductForViewDto = await _similarProductsAppService.GetSimilarProductForView(id);

            var model = new SimilarProductViewModel()
            {
                SimilarProduct = getSimilarProductForViewDto.SimilarProduct
                , ProductName = getSimilarProductForViewDto.ProductName 

                , ProductName2 = getSimilarProductForViewDto.ProductName2 

            };

            return PartialView("_ViewSimilarProductModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_SimilarProducts_Create, AppPermissions.Pages_Administration_SimilarProducts_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SimilarProductProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SimilarProductProductLookupTableModal", viewModel);
        }

    }
}