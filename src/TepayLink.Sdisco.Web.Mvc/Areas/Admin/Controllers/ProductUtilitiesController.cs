using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductUtilities;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductUtilities)]
    public class ProductUtilitiesController : SdiscoControllerBase
    {
        private readonly IProductUtilitiesAppService _productUtilitiesAppService;

        public ProductUtilitiesController(IProductUtilitiesAppService productUtilitiesAppService)
        {
            _productUtilitiesAppService = productUtilitiesAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductUtilitiesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Create, AppPermissions.Pages_Administration_ProductUtilities_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductUtilityForEditOutput getProductUtilityForEditOutput;

			if (id.HasValue){
				getProductUtilityForEditOutput = await _productUtilitiesAppService.GetProductUtilityForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductUtilityForEditOutput = new GetProductUtilityForEditOutput{
					ProductUtility = new CreateOrEditProductUtilityDto()
				};
			}

            var viewModel = new CreateOrEditProductUtilityModalViewModel()
            {
				ProductUtility = getProductUtilityForEditOutput.ProductUtility,
					ProductName = getProductUtilityForEditOutput.ProductName,
					UtilityName = getProductUtilityForEditOutput.UtilityName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductUtilityModal(long id)
        {
			var getProductUtilityForViewDto = await _productUtilitiesAppService.GetProductUtilityForView(id);

            var model = new ProductUtilityViewModel()
            {
                ProductUtility = getProductUtilityForViewDto.ProductUtility
                , ProductName = getProductUtilityForViewDto.ProductName 

                , UtilityName = getProductUtilityForViewDto.UtilityName 

            };

            return PartialView("_ViewProductUtilityModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Create, AppPermissions.Pages_Administration_ProductUtilities_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductUtilityProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductUtilityProductLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Create, AppPermissions.Pages_Administration_ProductUtilities_Edit)]
        public PartialViewResult UtilityLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ProductUtilityUtilityLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductUtilityUtilityLookupTableModal", viewModel);
        }

    }
}