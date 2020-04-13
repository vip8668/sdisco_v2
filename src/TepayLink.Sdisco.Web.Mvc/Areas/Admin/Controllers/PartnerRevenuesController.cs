using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.PartnerRevenues;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.KOL.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_PartnerRevenues)]
    public class PartnerRevenuesController : SdiscoControllerBase
    {
        private readonly IPartnerRevenuesAppService _partnerRevenuesAppService;

        public PartnerRevenuesController(IPartnerRevenuesAppService partnerRevenuesAppService)
        {
            _partnerRevenuesAppService = partnerRevenuesAppService;
        }

        public ActionResult Index()
        {
            var model = new PartnerRevenuesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_PartnerRevenues_Create, AppPermissions.Pages_Administration_PartnerRevenues_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetPartnerRevenueForEditOutput getPartnerRevenueForEditOutput;

			if (id.HasValue){
				getPartnerRevenueForEditOutput = await _partnerRevenuesAppService.GetPartnerRevenueForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getPartnerRevenueForEditOutput = new GetPartnerRevenueForEditOutput{
					PartnerRevenue = new CreateOrEditPartnerRevenueDto()
				};
			}

            var viewModel = new CreateOrEditPartnerRevenueModalViewModel()
            {
				PartnerRevenue = getPartnerRevenueForEditOutput.PartnerRevenue
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewPartnerRevenueModal(long id)
        {
			var getPartnerRevenueForViewDto = await _partnerRevenuesAppService.GetPartnerRevenueForView(id);

            var model = new PartnerRevenueViewModel()
            {
                PartnerRevenue = getPartnerRevenueForViewDto.PartnerRevenue
            };

            return PartialView("_ViewPartnerRevenueModal", model);
        }


    }
}