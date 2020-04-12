using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Partners;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Partners)]
    public class PartnersController : SdiscoControllerBase
    {
        private readonly IPartnersAppService _partnersAppService;

        public PartnersController(IPartnersAppService partnersAppService)
        {
            _partnersAppService = partnersAppService;
        }

        public ActionResult Index()
        {
            var model = new PartnersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Partners_Create, AppPermissions.Pages_Partners_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetPartnerForEditOutput getPartnerForEditOutput;

			if (id.HasValue){
				getPartnerForEditOutput = await _partnersAppService.GetPartnerForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getPartnerForEditOutput = new GetPartnerForEditOutput{
					Partner = new CreateOrEditPartnerDto()
				};
			}

            var viewModel = new CreateOrEditPartnerModalViewModel()
            {
				Partner = getPartnerForEditOutput.Partner,
					UserName = getPartnerForEditOutput.UserName,
					DetinationName = getPartnerForEditOutput.DetinationName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewPartnerModal(long id)
        {
			var getPartnerForViewDto = await _partnersAppService.GetPartnerForView(id);

            var model = new PartnerViewModel()
            {
                Partner = getPartnerForViewDto.Partner
                , UserName = getPartnerForViewDto.UserName 

                , DetinationName = getPartnerForViewDto.DetinationName 

            };

            return PartialView("_ViewPartnerModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Partners_Create, AppPermissions.Pages_Partners_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PartnerUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PartnerUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Partners_Create, AppPermissions.Pages_Partners_Edit)]
        public PartialViewResult DetinationLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PartnerDetinationLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PartnerDetinationLookupTableModal", viewModel);
        }

    }
}