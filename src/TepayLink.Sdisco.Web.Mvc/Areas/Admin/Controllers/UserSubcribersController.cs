using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.UserSubcribers;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserSubcribers)]
    public class UserSubcribersController : SdiscoControllerBase
    {
        private readonly IUserSubcribersAppService _userSubcribersAppService;

        public UserSubcribersController(IUserSubcribersAppService userSubcribersAppService)
        {
            _userSubcribersAppService = userSubcribersAppService;
        }

        public ActionResult Index()
        {
            var model = new UserSubcribersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserSubcribers_Create, AppPermissions.Pages_Administration_UserSubcribers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetUserSubcriberForEditOutput getUserSubcriberForEditOutput;

			if (id.HasValue){
				getUserSubcriberForEditOutput = await _userSubcribersAppService.GetUserSubcriberForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getUserSubcriberForEditOutput = new GetUserSubcriberForEditOutput{
					UserSubcriber = new CreateOrEditUserSubcriberDto()
				};
			}

            var viewModel = new CreateOrEditUserSubcriberModalViewModel()
            {
				UserSubcriber = getUserSubcriberForEditOutput.UserSubcriber
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserSubcriberModal(long id)
        {
			var getUserSubcriberForViewDto = await _userSubcribersAppService.GetUserSubcriberForView(id);

            var model = new UserSubcriberViewModel()
            {
                UserSubcriber = getUserSubcriberForViewDto.UserSubcriber
            };

            return PartialView("_ViewUserSubcriberModal", model);
        }


    }
}