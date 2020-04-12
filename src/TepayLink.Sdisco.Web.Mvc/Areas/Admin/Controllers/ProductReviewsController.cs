using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ProductReviews;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductReviews)]
    public class ProductReviewsController : SdiscoControllerBase
    {
        private readonly IProductReviewsAppService _productReviewsAppService;

        public ProductReviewsController(IProductReviewsAppService productReviewsAppService)
        {
            _productReviewsAppService = productReviewsAppService;
        }

        public ActionResult Index()
        {
            var model = new ProductReviewsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductReviews_Create, AppPermissions.Pages_Administration_ProductReviews_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetProductReviewForEditOutput getProductReviewForEditOutput;

			if (id.HasValue){
				getProductReviewForEditOutput = await _productReviewsAppService.GetProductReviewForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getProductReviewForEditOutput = new GetProductReviewForEditOutput{
					ProductReview = new CreateOrEditProductReviewDto()
				};
			}

            var viewModel = new CreateOrEditProductReviewModalViewModel()
            {
				ProductReview = getProductReviewForEditOutput.ProductReview,
					ProductName = getProductReviewForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductReviewModal(long id)
        {
			var getProductReviewForViewDto = await _productReviewsAppService.GetProductReviewForView(id);

            var model = new ProductReviewViewModel()
            {
                ProductReview = getProductReviewForViewDto.ProductReview
                , ProductName = getProductReviewForViewDto.ProductName 

            };

            return PartialView("_ViewProductReviewModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductReviews_Create, AppPermissions.Pages_Administration_ProductReviews_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ProductReviewProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductReviewProductLookupTableModal", viewModel);
        }

    }
}