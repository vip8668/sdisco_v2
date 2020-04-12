using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.UserDefaultCashoutMethodTypes;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Cashout.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes)]
    public class UserDefaultCashoutMethodTypesController : SdiscoControllerBase
    {
        private readonly IUserDefaultCashoutMethodTypesAppService _userDefaultCashoutMethodTypesAppService;

        public UserDefaultCashoutMethodTypesController(IUserDefaultCashoutMethodTypesAppService userDefaultCashoutMethodTypesAppService)
        {
            _userDefaultCashoutMethodTypesAppService = userDefaultCashoutMethodTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new UserDefaultCashoutMethodTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Create, AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetUserDefaultCashoutMethodTypeForEditOutput getUserDefaultCashoutMethodTypeForEditOutput;

			if (id.HasValue){
				getUserDefaultCashoutMethodTypeForEditOutput = await _userDefaultCashoutMethodTypesAppService.GetUserDefaultCashoutMethodTypeForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getUserDefaultCashoutMethodTypeForEditOutput = new GetUserDefaultCashoutMethodTypeForEditOutput{
					UserDefaultCashoutMethodType = new CreateOrEditUserDefaultCashoutMethodTypeDto()
				};
			}

            var viewModel = new CreateOrEditUserDefaultCashoutMethodTypeModalViewModel()
            {
				UserDefaultCashoutMethodType = getUserDefaultCashoutMethodTypeForEditOutput.UserDefaultCashoutMethodType,
					CashoutMethodTypeTitle = getUserDefaultCashoutMethodTypeForEditOutput.CashoutMethodTypeTitle,
					UserName = getUserDefaultCashoutMethodTypeForEditOutput.UserName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserDefaultCashoutMethodTypeModal(long id)
        {
			var getUserDefaultCashoutMethodTypeForViewDto = await _userDefaultCashoutMethodTypesAppService.GetUserDefaultCashoutMethodTypeForView(id);

            var model = new UserDefaultCashoutMethodTypeViewModel()
            {
                UserDefaultCashoutMethodType = getUserDefaultCashoutMethodTypeForViewDto.UserDefaultCashoutMethodType
                , CashoutMethodTypeTitle = getUserDefaultCashoutMethodTypeForViewDto.CashoutMethodTypeTitle 

                , UserName = getUserDefaultCashoutMethodTypeForViewDto.UserName 

            };

            return PartialView("_ViewUserDefaultCashoutMethodTypeModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Create, AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Edit)]
        public PartialViewResult CashoutMethodTypeLookupTableModal(int? id, string displayName)
        {
            var viewModel = new UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Create, AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new UserDefaultCashoutMethodTypeUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserDefaultCashoutMethodTypeUserLookupTableModal", viewModel);
        }

    }
}