using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ShareTransactions;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.KOL.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ShareTransactions)]
    public class ShareTransactionsController : SdiscoControllerBase
    {
        private readonly IShareTransactionsAppService _shareTransactionsAppService;

        public ShareTransactionsController(IShareTransactionsAppService shareTransactionsAppService)
        {
            _shareTransactionsAppService = shareTransactionsAppService;
        }

        public ActionResult Index()
        {
            var model = new ShareTransactionsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ShareTransactions_Create, AppPermissions.Pages_Administration_ShareTransactions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetShareTransactionForEditOutput getShareTransactionForEditOutput;

			if (id.HasValue){
				getShareTransactionForEditOutput = await _shareTransactionsAppService.GetShareTransactionForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getShareTransactionForEditOutput = new GetShareTransactionForEditOutput{
					ShareTransaction = new CreateOrEditShareTransactionDto()
				};
			}

            var viewModel = new CreateOrEditShareTransactionModalViewModel()
            {
				ShareTransaction = getShareTransactionForEditOutput.ShareTransaction
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewShareTransactionModal(long id)
        {
			var getShareTransactionForViewDto = await _shareTransactionsAppService.GetShareTransactionForView(id);

            var model = new ShareTransactionViewModel()
            {
                ShareTransaction = getShareTransactionForViewDto.ShareTransaction
            };

            return PartialView("_ViewShareTransactionModal", model);
        }


    }
}