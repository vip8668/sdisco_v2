using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Utilities;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Utilities)]
    public class UtilitiesController : SdiscoControllerBase
    {
        private readonly IUtilitiesAppService _utilitiesAppService;

        public UtilitiesController(IUtilitiesAppService utilitiesAppService)
        {
            _utilitiesAppService = utilitiesAppService;
        }

        public ActionResult Index()
        {
            var model = new UtilitiesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Utilities_Create, AppPermissions.Pages_Administration_Utilities_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetUtilityForEditOutput getUtilityForEditOutput;

			if (id.HasValue){
				getUtilityForEditOutput = await _utilitiesAppService.GetUtilityForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getUtilityForEditOutput = new GetUtilityForEditOutput{
					Utility = new CreateOrEditUtilityDto()
				};
			}

            var viewModel = new CreateOrEditUtilityModalViewModel()
            {
				Utility = getUtilityForEditOutput.Utility
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUtilityModal(int id)
        {
			var getUtilityForViewDto = await _utilitiesAppService.GetUtilityForView(id);

            var model = new UtilityViewModel()
            {
                Utility = getUtilityForViewDto.Utility
            };

            return PartialView("_ViewUtilityModal", model);
        }


    }
}