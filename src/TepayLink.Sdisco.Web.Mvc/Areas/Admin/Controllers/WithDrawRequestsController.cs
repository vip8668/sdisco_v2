using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.WithDrawRequests;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_WithDrawRequests)]
    public class WithDrawRequestsController : SdiscoControllerBase
    {
        private readonly IWithDrawRequestsAppService _withDrawRequestsAppService;

        public WithDrawRequestsController(IWithDrawRequestsAppService withDrawRequestsAppService)
        {
            _withDrawRequestsAppService = withDrawRequestsAppService;
        }

        public ActionResult Index()
        {
            var model = new WithDrawRequestsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_WithDrawRequests_Create, AppPermissions.Pages_Administration_WithDrawRequests_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetWithDrawRequestForEditOutput getWithDrawRequestForEditOutput;

			if (id.HasValue){
				getWithDrawRequestForEditOutput = await _withDrawRequestsAppService.GetWithDrawRequestForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getWithDrawRequestForEditOutput = new GetWithDrawRequestForEditOutput{
					WithDrawRequest = new CreateOrEditWithDrawRequestDto()
				};
			}

            var viewModel = new CreateOrEditWithDrawRequestModalViewModel()
            {
				WithDrawRequest = getWithDrawRequestForEditOutput.WithDrawRequest
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewWithDrawRequestModal(long id)
        {
			var getWithDrawRequestForViewDto = await _withDrawRequestsAppService.GetWithDrawRequestForView(id);

            var model = new WithDrawRequestViewModel()
            {
                WithDrawRequest = getWithDrawRequestForViewDto.WithDrawRequest
            };

            return PartialView("_ViewWithDrawRequestModal", model);
        }


    }
}