using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Banks;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Banks)]
    public class BanksController : SdiscoControllerBase
    {
        private readonly IBanksAppService _banksAppService;

        public BanksController(IBanksAppService banksAppService)
        {
            _banksAppService = banksAppService;
        }

        public ActionResult Index()
        {
            var model = new BanksViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Banks_Create, AppPermissions.Pages_Administration_Banks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetBankForEditOutput getBankForEditOutput;

			if (id.HasValue){
				getBankForEditOutput = await _banksAppService.GetBankForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getBankForEditOutput = new GetBankForEditOutput{
					Bank = new CreateOrEditBankDto()
				};
			}

            var viewModel = new CreateOrEditBankModalViewModel()
            {
				Bank = getBankForEditOutput.Bank
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBankModal(int id)
        {
			var getBankForViewDto = await _banksAppService.GetBankForView(id);

            var model = new BankViewModel()
            {
                Bank = getBankForViewDto.Bank
            };

            return PartialView("_ViewBankModal", model);
        }


    }
}