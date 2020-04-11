using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Detinations;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Detinations)]
    public class DetinationsController : SdiscoControllerBase
    {
        private readonly IDetinationsAppService _detinationsAppService;

        public DetinationsController(IDetinationsAppService detinationsAppService)
        {
            _detinationsAppService = detinationsAppService;
        }

        public ActionResult Index()
        {
            var model = new DetinationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Detinations_Create, AppPermissions.Pages_Administration_Detinations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetDetinationForEditOutput getDetinationForEditOutput;

			if (id.HasValue){
				getDetinationForEditOutput = await _detinationsAppService.GetDetinationForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getDetinationForEditOutput = new GetDetinationForEditOutput{
					Detination = new CreateOrEditDetinationDto()
				};
			}

            var viewModel = new CreateOrEditDetinationModalViewModel()
            {
				Detination = getDetinationForEditOutput.Detination
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDetinationModal(long id)
        {
			var getDetinationForViewDto = await _detinationsAppService.GetDetinationForView(id);

            var model = new DetinationViewModel()
            {
                Detination = getDetinationForViewDto.Detination
            };

            return PartialView("_ViewDetinationModal", model);
        }


    }
}