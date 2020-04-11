using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.BankBranchs;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_BankBranchs)]
    public class BankBranchsController : SdiscoControllerBase
    {
        private readonly IBankBranchsAppService _bankBranchsAppService;

        public BankBranchsController(IBankBranchsAppService bankBranchsAppService)
        {
            _bankBranchsAppService = bankBranchsAppService;
        }

        public ActionResult Index()
        {
            var model = new BankBranchsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_BankBranchs_Create, AppPermissions.Pages_BankBranchs_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetBankBranchForEditOutput getBankBranchForEditOutput;

			if (id.HasValue){
				getBankBranchForEditOutput = await _bankBranchsAppService.GetBankBranchForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getBankBranchForEditOutput = new GetBankBranchForEditOutput{
					BankBranch = new CreateOrEditBankBranchDto()
				};
			}

            var viewModel = new CreateOrEditBankBranchModalViewModel()
            {
				BankBranch = getBankBranchForEditOutput.BankBranch,
					BankBankName = getBankBranchForEditOutput.BankBankName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBankBranchModal(int id)
        {
			var getBankBranchForViewDto = await _bankBranchsAppService.GetBankBranchForView(id);

            var model = new BankBranchViewModel()
            {
                BankBranch = getBankBranchForViewDto.BankBranch
                , BankBankName = getBankBranchForViewDto.BankBankName 

            };

            return PartialView("_ViewBankBranchModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BankBranchs_Create, AppPermissions.Pages_BankBranchs_Edit)]
        public PartialViewResult BankLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BankBranchBankLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BankBranchBankLookupTableModal", viewModel);
        }

    }
}