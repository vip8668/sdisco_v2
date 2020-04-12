using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductDetailCombos;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos)]
    public class ProductDetailCombosController : SdiscoControllerBase
    {
        private readonly IProductDetailCombosAppService _productDetailCombosAppService;

        public ProductDetailCombosController(IProductDetailCombosAppService productDetailCombosAppService)
        {
            _productDetailCombosAppService = productDetailCombosAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductDetailCombosViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Create, AppPermissions.Pages_Administration_ProductDetailCombos_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductDetailComboForEditOutput getProductDetailComboForEditOutput;

			if (id.HasValue){
				getProductDetailComboForEditOutput = await _productDetailCombosAppService.GetProductDetailComboForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductDetailComboForEditOutput = new GetProductDetailComboForEditOutput{
					ProductDetailCombo = new CreateOrEditProductDetailComboDto()
				};
			}

            var viewModel = new CreateOrEditProductDetailComboModalViewModel()
            {
				ProductDetailCombo = getProductDetailComboForEditOutput.ProductDetailCombo,
					ProductName = getProductDetailComboForEditOutput.ProductName,
					ProductDetailTitle = getProductDetailComboForEditOutput.ProductDetailTitle,
					ProductName2 = getProductDetailComboForEditOutput.ProductName2
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductDetailComboModal(long id)
        {
			var getProductDetailComboForViewDto = await _productDetailCombosAppService.GetProductDetailComboForView(id);

            var model = new ProductDetailComboViewModel()
            {
                ProductDetailCombo = getProductDetailComboForViewDto.ProductDetailCombo
                , ProductName = getProductDetailComboForViewDto.ProductName 

                , ProductDetailTitle = getProductDetailComboForViewDto.ProductDetailTitle 

                , ProductName2 = getProductDetailComboForViewDto.ProductName2 

            };

            return PartialView("_ViewProductDetailComboModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Create, AppPermissions.Pages_Administration_ProductDetailCombos_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductDetailComboProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductDetailComboProductLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Create, AppPermissions.Pages_Administration_ProductDetailCombos_Edit)]
        public PartialViewResult ProductDetailLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductDetailComboProductDetailLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductDetailComboProductDetailLookupTableModal", viewModel);
        }

    }
}