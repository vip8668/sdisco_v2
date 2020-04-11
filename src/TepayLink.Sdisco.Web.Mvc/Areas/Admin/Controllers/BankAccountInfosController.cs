using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BankAccountInfos;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_BankAccountInfos)]
    public class BankAccountInfosController : SdiscoControllerBase
    {
        private readonly IBankAccountInfosAppService _bankAccountInfosAppService;

        public BankAccountInfosController(IBankAccountInfosAppService bankAccountInfosAppService)
        {
            _bankAccountInfosAppService = bankAccountInfosAppService;
        }

        public ActionResult Index()
        {
            var model = new BankAccountInfosViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Create, AppPermissions.Pages_Administration_BankAccountInfos_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetBankAccountInfoForEditOutput getBankAccountInfoForEditOutput;

			if (id.HasValue){
				getBankAccountInfoForEditOutput = await _bankAccountInfosAppService.GetBankAccountInfoForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getBankAccountInfoForEditOutput = new GetBankAccountInfoForEditOutput{
					BankAccountInfo = new CreateOrEditBankAccountInfoDto()
				};
			}

            var viewModel = new CreateOrEditBankAccountInfoModalViewModel()
            {
				BankAccountInfo = getBankAccountInfoForEditOutput.BankAccountInfo,
					BankBankName = getBankAccountInfoForEditOutput.BankBankName,
					BankBranchBranchName = getBankAccountInfoForEditOutput.BankBranchBranchName,
					UserName = getBankAccountInfoForEditOutput.UserName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBankAccountInfoModal(long id)
        {
			var getBankAccountInfoForViewDto = await _bankAccountInfosAppService.GetBankAccountInfoForView(id);

            var model = new BankAccountInfoViewModel()
            {
                BankAccountInfo = getBankAccountInfoForViewDto.BankAccountInfo
                , BankBankName = getBankAccountInfoForViewDto.BankBankName 

                , BankBranchBranchName = getBankAccountInfoForViewDto.BankBranchBranchName 

                , UserName = getBankAccountInfoForViewDto.UserName 

            };

            return PartialView("_ViewBankAccountInfoModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Create, AppPermissions.Pages_Administration_BankAccountInfos_Edit)]
        public PartialViewResult BankLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BankAccountInfoBankLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BankAccountInfoBankLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Create, AppPermissions.Pages_Administration_BankAccountInfos_Edit)]
        public PartialViewResult BankBranchLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BankAccountInfoBankBranchLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BankAccountInfoBankBranchLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Create, AppPermissions.Pages_Administration_BankAccountInfos_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BankAccountInfoUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BankAccountInfoUserLookupTableModal", viewModel);
        }

    }
}