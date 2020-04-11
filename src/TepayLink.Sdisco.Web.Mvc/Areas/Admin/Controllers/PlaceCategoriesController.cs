using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.PlaceCategories;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_PlaceCategories)]
    public class PlaceCategoriesController : SdiscoControllerBase
    {
        private readonly IPlaceCategoriesAppService _placeCategoriesAppService;

        public PlaceCategoriesController(IPlaceCategoriesAppService placeCategoriesAppService)
        {
            _placeCategoriesAppService = placeCategoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new PlaceCategoriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_PlaceCategories_Create, AppPermissions.Pages_Administration_PlaceCategories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetPlaceCategoryForEditOutput getPlaceCategoryForEditOutput;

			if (id.HasValue){
				getPlaceCategoryForEditOutput = await _placeCategoriesAppService.GetPlaceCategoryForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getPlaceCategoryForEditOutput = new GetPlaceCategoryForEditOutput{
					PlaceCategory = new CreateOrEditPlaceCategoryDto()
				};
			}

            var viewModel = new CreateOrEditPlaceCategoryModalViewModel()
            {
				PlaceCategory = getPlaceCategoryForEditOutput.PlaceCategory
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewPlaceCategoryModal(int id)
        {
			var getPlaceCategoryForViewDto = await _placeCategoriesAppService.GetPlaceCategoryForView(id);

            var model = new PlaceCategoryViewModel()
            {
                PlaceCategory = getPlaceCategoryForViewDto.PlaceCategory
            };

            return PartialView("_ViewPlaceCategoryModal", model);
        }


    }
}