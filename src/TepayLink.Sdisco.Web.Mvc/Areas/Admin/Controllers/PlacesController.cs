using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Places;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Places)]
    public class PlacesController : SdiscoControllerBase
    {
        private readonly IPlacesAppService _placesAppService;

        public PlacesController(IPlacesAppService placesAppService)
        {
            _placesAppService = placesAppService;
        }

        public ActionResult Index()
        {
            var model = new PlacesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Places_Create, AppPermissions.Pages_Administration_Places_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetPlaceForEditOutput getPlaceForEditOutput;

			if (id.HasValue){
				getPlaceForEditOutput = await _placesAppService.GetPlaceForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getPlaceForEditOutput = new GetPlaceForEditOutput{
					Place = new CreateOrEditPlaceDto()
				};
			}

            var viewModel = new CreateOrEditPlaceModalViewModel()
            {
				Place = getPlaceForEditOutput.Place,
					DetinationName = getPlaceForEditOutput.DetinationName,
					PlaceCategoryName = getPlaceForEditOutput.PlaceCategoryName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewPlaceModal(long id)
        {
			var getPlaceForViewDto = await _placesAppService.GetPlaceForView(id);

            var model = new PlaceViewModel()
            {
                Place = getPlaceForViewDto.Place
                , DetinationName = getPlaceForViewDto.DetinationName 

                , PlaceCategoryName = getPlaceForViewDto.PlaceCategoryName 

            };

            return PartialView("_ViewPlaceModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Places_Create, AppPermissions.Pages_Administration_Places_Edit)]
        public PartialViewResult DetinationLookupTableModal(long? id, string displayName)
        {
            var viewModel = new PlaceDetinationLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PlaceDetinationLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Places_Create, AppPermissions.Pages_Administration_Places_Edit)]
        public PartialViewResult PlaceCategoryLookupTableModal(int? id, string displayName)
        {
            var viewModel = new PlacePlaceCategoryLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_PlacePlaceCategoryLookupTableModal", viewModel);
        }

    }
}