using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.CashoutMethodTypes;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Cashout.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes)]
    public class CashoutMethodTypesController : SdiscoControllerBase
    {
        private readonly ICashoutMethodTypesAppService _cashoutMethodTypesAppService;

        public CashoutMethodTypesController(ICashoutMethodTypesAppService cashoutMethodTypesAppService)
        {
            _cashoutMethodTypesAppService = cashoutMethodTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new CashoutMethodTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes_Create, AppPermissions.Pages_Administration_CashoutMethodTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetCashoutMethodTypeForEditOutput getCashoutMethodTypeForEditOutput;

			if (id.HasValue){
				getCashoutMethodTypeForEditOutput = await _cashoutMethodTypesAppService.GetCashoutMethodTypeForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getCashoutMethodTypeForEditOutput = new GetCashoutMethodTypeForEditOutput{
					CashoutMethodType = new CreateOrEditCashoutMethodTypeDto()
				};
			}

            var viewModel = new CreateOrEditCashoutMethodTypeModalViewModel()
            {
				CashoutMethodType = getCashoutMethodTypeForEditOutput.CashoutMethodType
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCashoutMethodTypeModal(int id)
        {
			var getCashoutMethodTypeForViewDto = await _cashoutMethodTypesAppService.GetCashoutMethodTypeForView(id);

            var model = new CashoutMethodTypeViewModel()
            {
                CashoutMethodType = getCashoutMethodTypeForViewDto.CashoutMethodType
            };

            return PartialView("_ViewCashoutMethodTypeModal", model);
        }


    }
}