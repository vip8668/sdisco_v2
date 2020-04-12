using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Currencies;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Currencies)]
    public class CurrenciesController : SdiscoControllerBase
    {
        private readonly ICurrenciesAppService _currenciesAppService;

        public CurrenciesController(ICurrenciesAppService currenciesAppService)
        {
            _currenciesAppService = currenciesAppService;
        }

        public ActionResult Index()
        {
            var model = new CurrenciesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Currencies_Create, AppPermissions.Pages_Administration_Currencies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetCurrencyForEditOutput getCurrencyForEditOutput;

			if (id.HasValue){
				getCurrencyForEditOutput = await _currenciesAppService.GetCurrencyForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getCurrencyForEditOutput = new GetCurrencyForEditOutput{
					Currency = new CreateOrEditCurrencyDto()
				};
			}

            var viewModel = new CreateOrEditCurrencyModalViewModel()
            {
				Currency = getCurrencyForEditOutput.Currency
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCurrencyModal(int id)
        {
			var getCurrencyForViewDto = await _currenciesAppService.GetCurrencyForView(id);

            var model = new CurrencyViewModel()
            {
                Currency = getCurrencyForViewDto.Currency
            };

            return PartialView("_ViewCurrencyModal", model);
        }


    }
}