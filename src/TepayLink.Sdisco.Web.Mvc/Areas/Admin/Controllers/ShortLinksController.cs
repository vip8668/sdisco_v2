using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ShortLinks;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Affiliate;
using TepayLink.Sdisco.Affiliate.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ShortLinks)]
    public class ShortLinksController : SdiscoControllerBase
    {
        private readonly IShortLinksAppService _shortLinksAppService;

        public ShortLinksController(IShortLinksAppService shortLinksAppService)
        {
            _shortLinksAppService = shortLinksAppService;
        }

        public ActionResult Index()
        {
            var model = new ShortLinksViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ShortLinks_Create, AppPermissions.Pages_Administration_ShortLinks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetShortLinkForEditOutput getShortLinkForEditOutput;

			if (id.HasValue){
				getShortLinkForEditOutput = await _shortLinksAppService.GetShortLinkForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getShortLinkForEditOutput = new GetShortLinkForEditOutput{
					ShortLink = new CreateOrEditShortLinkDto()
				};
			}

            var viewModel = new CreateOrEditShortLinkModalViewModel()
            {
				ShortLink = getShortLinkForEditOutput.ShortLink
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewShortLinkModal(long id)
        {
			var getShortLinkForViewDto = await _shortLinksAppService.GetShortLinkForView(id);

            var model = new ShortLinkViewModel()
            {
                ShortLink = getShortLinkForViewDto.ShortLink
            };

            return PartialView("_ViewShortLinkModal", model);
        }


    }
}