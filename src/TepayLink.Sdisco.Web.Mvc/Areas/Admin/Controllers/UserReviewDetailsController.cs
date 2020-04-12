using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.UserReviewDetails;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserReviewDetails)]
    public class UserReviewDetailsController : SdiscoControllerBase
    {
        private readonly IUserReviewDetailsAppService _userReviewDetailsAppService;

        public UserReviewDetailsController(IUserReviewDetailsAppService userReviewDetailsAppService)
        {
            _userReviewDetailsAppService = userReviewDetailsAppService;
        }

        public ActionResult Index()
        {
            var model = new UserReviewDetailsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserReviewDetails_Create, AppPermissions.Pages_Administration_UserReviewDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetUserReviewDetailForEditOutput getUserReviewDetailForEditOutput;

			if (id.HasValue){
				getUserReviewDetailForEditOutput = await _userReviewDetailsAppService.GetUserReviewDetailForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getUserReviewDetailForEditOutput = new GetUserReviewDetailForEditOutput{
					UserReviewDetail = new CreateOrEditUserReviewDetailDto()
				};
			}

            var viewModel = new CreateOrEditUserReviewDetailModalViewModel()
            {
				UserReviewDetail = getUserReviewDetailForEditOutput.UserReviewDetail
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserReviewDetailModal(long id)
        {
			var getUserReviewDetailForViewDto = await _userReviewDetailsAppService.GetUserReviewDetailForView(id);

            var model = new UserReviewDetailViewModel()
            {
                UserReviewDetail = getUserReviewDetailForViewDto.UserReviewDetail
            };

            return PartialView("_ViewUserReviewDetailModal", model);
        }


    }
}