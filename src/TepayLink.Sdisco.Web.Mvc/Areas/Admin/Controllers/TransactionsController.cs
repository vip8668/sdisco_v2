using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Transactions;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Transactions)]
    public class TransactionsController : SdiscoControllerBase
    {
        private readonly ITransactionsAppService _transactionsAppService;

        public TransactionsController(ITransactionsAppService transactionsAppService)
        {
            _transactionsAppService = transactionsAppService;
        }

        public ActionResult Index()
        {
            var model = new TransactionsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Transactions_Create, AppPermissions.Pages_Administration_Transactions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetTransactionForEditOutput getTransactionForEditOutput;

			if (id.HasValue){
				getTransactionForEditOutput = await _transactionsAppService.GetTransactionForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getTransactionForEditOutput = new GetTransactionForEditOutput{
					Transaction = new CreateOrEditTransactionDto()
				};
			}

            var viewModel = new CreateOrEditTransactionModalViewModel()
            {
				Transaction = getTransactionForEditOutput.Transaction
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTransactionModal(long id)
        {
			var getTransactionForViewDto = await _transactionsAppService.GetTransactionForView(id);

            var model = new TransactionViewModel()
            {
                Transaction = getTransactionForViewDto.Transaction
            };

            return PartialView("_ViewTransactionModal", model);
        }


    }
}