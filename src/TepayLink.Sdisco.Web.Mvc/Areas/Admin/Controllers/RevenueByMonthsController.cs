using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.RevenueByMonths;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Reports;
using TepayLink.Sdisco.Reports.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RevenueByMonths)]
    public class RevenueByMonthsController : SdiscoControllerBase
    {
        private readonly IRevenueByMonthsAppService _revenueByMonthsAppService;

        public RevenueByMonthsController(IRevenueByMonthsAppService revenueByMonthsAppService)
        {
            _revenueByMonthsAppService = revenueByMonthsAppService;
        }

        public ActionResult Index()
        {
            var model = new RevenueByMonthsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RevenueByMonths_Create, AppPermissions.Pages_Administration_RevenueByMonths_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetRevenueByMonthForEditOutput getRevenueByMonthForEditOutput;

			if (id.HasValue){
				getRevenueByMonthForEditOutput = await _revenueByMonthsAppService.GetRevenueByMonthForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getRevenueByMonthForEditOutput = new GetRevenueByMonthForEditOutput{
					RevenueByMonth = new CreateOrEditRevenueByMonthDto()
				};
				getRevenueByMonthForEditOutput.RevenueByMonth.Date = DateTime.Now;
			}

            var viewModel = new CreateOrEditRevenueByMonthModalViewModel()
            {
				RevenueByMonth = getRevenueByMonthForEditOutput.RevenueByMonth
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRevenueByMonthModal(long id)
        {
			var getRevenueByMonthForViewDto = await _revenueByMonthsAppService.GetRevenueByMonthForView(id);

            var model = new RevenueByMonthViewModel()
            {
                RevenueByMonth = getRevenueByMonthForViewDto.RevenueByMonth
            };

            return PartialView("_ViewRevenueByMonthModal", model);
        }


    }
}