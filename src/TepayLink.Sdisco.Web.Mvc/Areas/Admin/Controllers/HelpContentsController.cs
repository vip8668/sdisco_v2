using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.HelpContents;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Help;
using TepayLink.Sdisco.Help.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_HelpContents)]
    public class HelpContentsController : SdiscoControllerBase
    {
        private readonly IHelpContentsAppService _helpContentsAppService;

        public HelpContentsController(IHelpContentsAppService helpContentsAppService)
        {
            _helpContentsAppService = helpContentsAppService;
        }

        public ActionResult Index()
        {
            var model = new HelpContentsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_HelpContents_Create, AppPermissions.Pages_Administration_HelpContents_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetHelpContentForEditOutput getHelpContentForEditOutput;

			if (id.HasValue){
				getHelpContentForEditOutput = await _helpContentsAppService.GetHelpContentForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getHelpContentForEditOutput = new GetHelpContentForEditOutput{
					HelpContent = new CreateOrEditHelpContentDto()
				};
			}

            var viewModel = new CreateOrEditHelpContentModalViewModel()
            {
				HelpContent = getHelpContentForEditOutput.HelpContent,
					HelpCategoryCategoryName = getHelpContentForEditOutput.HelpCategoryCategoryName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewHelpContentModal(long id)
        {
			var getHelpContentForViewDto = await _helpContentsAppService.GetHelpContentForView(id);

            var model = new HelpContentViewModel()
            {
                HelpContent = getHelpContentForViewDto.HelpContent
                , HelpCategoryCategoryName = getHelpContentForViewDto.HelpCategoryCategoryName 

            };

            return PartialView("_ViewHelpContentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_HelpContents_Create, AppPermissions.Pages_Administration_HelpContents_Edit)]
        public PartialViewResult HelpCategoryLookupTableModal(long? id, string displayName)
        {
            var viewModel = new HelpContentHelpCategoryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_HelpContentHelpCategoryLookupTableModal", viewModel);
        }

    }
}