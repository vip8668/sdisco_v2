using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.SaveItems;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_SaveItems)]
    public class SaveItemsController : SdiscoControllerBase
    {
        private readonly ISaveItemsAppService _saveItemsAppService;

        public SaveItemsController(ISaveItemsAppService saveItemsAppService)
        {
            _saveItemsAppService = saveItemsAppService;
        }

        public ActionResult Index()
        {
            var model = new SaveItemsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_SaveItems_Create, AppPermissions.Pages_Administration_SaveItems_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetSaveItemForEditOutput getSaveItemForEditOutput;

			if (id.HasValue){
				getSaveItemForEditOutput = await _saveItemsAppService.GetSaveItemForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getSaveItemForEditOutput = new GetSaveItemForEditOutput{
					SaveItem = new CreateOrEditSaveItemDto()
				};
			}

            var viewModel = new CreateOrEditSaveItemModalViewModel()
            {
				SaveItem = getSaveItemForEditOutput.SaveItem,
					ProductName = getSaveItemForEditOutput.ProductName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSaveItemModal(long id)
        {
			var getSaveItemForViewDto = await _saveItemsAppService.GetSaveItemForView(id);

            var model = new SaveItemViewModel()
            {
                SaveItem = getSaveItemForViewDto.SaveItem
                , ProductName = getSaveItemForViewDto.ProductName 

            };

            return PartialView("_ViewSaveItemModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_SaveItems_Create, AppPermissions.Pages_Administration_SaveItems_Edit)]
        public PartialViewResult ProductLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SaveItemProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SaveItemProductLookupTableModal", viewModel);
        }

    }
}