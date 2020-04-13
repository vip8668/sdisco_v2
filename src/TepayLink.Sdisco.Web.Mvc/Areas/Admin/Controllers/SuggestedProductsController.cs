using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.SuggestedProducts;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_SuggestedProducts)]
    public class SuggestedProductsController : SdiscoControllerBase
    {
        private readonly ISuggestedProductsAppService _suggestedProductsAppService;

        public SuggestedProductsController(ISuggestedProductsAppService suggestedProductsAppService)
        {
            _suggestedProductsAppService = suggestedProductsAppService;
        }

        public ActionResult Index()
        {
            var model = new SuggestedProductsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_SuggestedProducts_Create, AppPermissions.Pages_Administration_SuggestedProducts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetSuggestedProductForEditOutput getSuggestedProductForEditOutput;

			if (id.HasValue){
				getSuggestedProductForEditOutput = await _suggestedProductsAppService.GetSuggestedProductForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getSuggestedProductForEditOutput = new GetSuggestedProductForEditOutput{
					SuggestedProduct = new CreateOrEditSuggestedProductDto()
				};
			}

            var viewModel = new CreateOrEditSuggestedProductModalViewModel()
            {
				SuggestedProduct = getSuggestedProductForEditOutput.SuggestedProduct,
					ProductName = getSuggestedProductForEditOutput.ProductName,
					ProductName2 = getSuggestedProductForEditOutput.ProductName2
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSuggestedProductModal(long id)
        {
			var getSuggestedProductForViewDto = await _suggestedProductsAppService.GetSuggestedProductForView(id);

            var model = new SuggestedProductViewModel()
            {
                SuggestedProduct = getSuggestedProductForViewDto.SuggestedProduct
                , ProductName = getSuggestedProductForViewDto.ProductName 

                , ProductName2 = getSuggestedProductForViewDto.ProductName2 

            };

            return PartialView("_ViewSuggestedProductModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_SuggestedProducts_Create, AppPermissions.Pages_Administration_SuggestedProducts_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SuggestedProductProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SuggestedProductProductLookupTableModal", viewModel);
        }

    }
}