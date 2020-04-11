using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.HelpCategories;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Help;
using TepayLink.Sdisco.Help.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_HelpCategories)]
    public class HelpCategoriesController : SdiscoControllerBase
    {
        private readonly IHelpCategoriesAppService _helpCategoriesAppService;

        public HelpCategoriesController(IHelpCategoriesAppService helpCategoriesAppService)
        {
            _helpCategoriesAppService = helpCategoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new HelpCategoriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_HelpCategories_Create, AppPermissions.Pages_Administration_HelpCategories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetHelpCategoryForEditOutput getHelpCategoryForEditOutput;

			if (id.HasValue){
				getHelpCategoryForEditOutput = await _helpCategoriesAppService.GetHelpCategoryForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getHelpCategoryForEditOutput = new GetHelpCategoryForEditOutput{
					HelpCategory = new CreateOrEditHelpCategoryDto()
				};
			}

            var viewModel = new CreateOrEditHelpCategoryModalViewModel()
            {
				HelpCategory = getHelpCategoryForEditOutput.HelpCategory
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewHelpCategoryModal(long id)
        {
			var getHelpCategoryForViewDto = await _helpCategoriesAppService.GetHelpCategoryForView(id);

            var model = new HelpCategoryViewModel()
            {
                HelpCategory = getHelpCategoryForViewDto.HelpCategory
            };

            return PartialView("_ViewHelpCategoryModal", model);
        }


    }
}