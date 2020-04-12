using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.UserReviews;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserReviews)]
    public class UserReviewsController : SdiscoControllerBase
    {
        private readonly IUserReviewsAppService _userReviewsAppService;

        public UserReviewsController(IUserReviewsAppService userReviewsAppService)
        {
            _userReviewsAppService = userReviewsAppService;
        }

        public ActionResult Index()
        {
            var model = new UserReviewsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserReviews_Create, AppPermissions.Pages_Administration_UserReviews_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetUserReviewForEditOutput getUserReviewForEditOutput;

			if (id.HasValue){
				getUserReviewForEditOutput = await _userReviewsAppService.GetUserReviewForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getUserReviewForEditOutput = new GetUserReviewForEditOutput{
					UserReview = new CreateOrEditUserReviewDto()
				};
			}

            var viewModel = new CreateOrEditUserReviewModalViewModel()
            {
				UserReview = getUserReviewForEditOutput.UserReview
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserReviewModal(int id)
        {
			var getUserReviewForViewDto = await _userReviewsAppService.GetUserReviewForView(id);

            var model = new UserReviewViewModel()
            {
                UserReview = getUserReviewForViewDto.UserReview
            };

            return PartialView("_ViewUserReviewModal", model);
        }


    }
}