using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductReviewDetails;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails)]
    public class ProductReviewDetailsController : SdiscoControllerBase
    {
        private readonly IProductReviewDetailsAppService _productReviewDetailsAppService;

        public ProductReviewDetailsController(IProductReviewDetailsAppService productReviewDetailsAppService)
        {
            _productReviewDetailsAppService = productReviewDetailsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductReviewDetailsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Create, AppPermissions.Pages_Administration_ProductReviewDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductReviewDetailForEditOutput getProductReviewDetailForEditOutput;

			if (id.HasValue){
				getProductReviewDetailForEditOutput = await _productReviewDetailsAppService.GetProductReviewDetailForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductReviewDetailForEditOutput = new GetProductReviewDetailForEditOutput{
					ProductReviewDetail = new CreateOrEditProductReviewDetailDto()
				};
			}

            var viewModel = new CreateOrEditProductReviewDetailModalViewModel()
            {
				ProductReviewDetail = getProductReviewDetailForEditOutput.ProductReviewDetail,
					ProductName = getProductReviewDetailForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductReviewDetailModal(long id)
        {
			var getProductReviewDetailForViewDto = await _productReviewDetailsAppService.GetProductReviewDetailForView(id);

            var model = new ProductReviewDetailViewModel()
            {
                ProductReviewDetail = getProductReviewDetailForViewDto.ProductReviewDetail
                , ProductName = getProductReviewDetailForViewDto.ProductName 

            };

            return PartialView("_ViewProductReviewDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Create, AppPermissions.Pages_Administration_ProductReviewDetails_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductReviewDetailProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductReviewDetailProductLookupTableModal", viewModel);
        }

    }
}