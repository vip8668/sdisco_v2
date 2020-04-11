using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Categories;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Categories)]
    public class CategoriesController : SdiscoControllerBase
    {
        private readonly ICategoriesAppService _categoriesAppService;

        public CategoriesController(ICategoriesAppService categoriesAppService)
        {
            _categoriesAppService = categoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new CategoriesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Categories_Create, AppPermissions.Pages_Administration_Categories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetCategoryForEditOutput getCategoryForEditOutput;

			if (id.HasValue){
				getCategoryForEditOutput = await _categoriesAppService.GetCategoryForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getCategoryForEditOutput = new GetCategoryForEditOutput{
					Category = new CreateOrEditCategoryDto()
				};
			}

            var viewModel = new CreateOrEditCategoryModalViewModel()
            {
				Category = getCategoryForEditOutput.Category
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCategoryModal(int id)
        {
			var getCategoryForViewDto = await _categoriesAppService.GetCategoryForView(id);

            var model = new CategoryViewModel()
            {
                Category = getCategoryForViewDto.Category
            };

            return PartialView("_ViewCategoryModal", model);
        }


    }
}