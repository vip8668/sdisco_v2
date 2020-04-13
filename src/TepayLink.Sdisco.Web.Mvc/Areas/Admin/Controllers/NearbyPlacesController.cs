using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.NearbyPlaces;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Places;
using TepayLink.Sdisco.Places.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_NearbyPlaces)]
    public class NearbyPlacesController : SdiscoControllerBase
    {
        private readonly INearbyPlacesAppService _nearbyPlacesAppService;

        public NearbyPlacesController(INearbyPlacesAppService nearbyPlacesAppService)
        {
            _nearbyPlacesAppService = nearbyPlacesAppService;
        }

        public ActionResult Index()
        {
            var model = new NearbyPlacesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_NearbyPlaces_Create, AppPermissions.Pages_Administration_NearbyPlaces_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetNearbyPlaceForEditOutput getNearbyPlaceForEditOutput;

			if (id.HasValue){
				getNearbyPlaceForEditOutput = await _nearbyPlacesAppService.GetNearbyPlaceForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getNearbyPlaceForEditOutput = new GetNearbyPlaceForEditOutput{
					NearbyPlace = new CreateOrEditNearbyPlaceDto()
				};
			}

            var viewModel = new CreateOrEditNearbyPlaceModalViewModel()
            {
				NearbyPlace = getNearbyPlaceForEditOutput.NearbyPlace,
					PlaceName = getNearbyPlaceForEditOutput.PlaceName,
					PlaceName2 = getNearbyPlaceForEditOutput.PlaceName2
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewNearbyPlaceModal(long id)
        {
			var getNearbyPlaceForViewDto = await _nearbyPlacesAppService.GetNearbyPlaceForView(id);

            var model = new NearbyPlaceViewModel()
            {
                NearbyPlace = getNearbyPlaceForViewDto.NearbyPlace
                , PlaceName = getNearbyPlaceForViewDto.PlaceName 

                , PlaceName2 = getNearbyPlaceForViewDto.PlaceName2 

            };

            return PartialView("_ViewNearbyPlaceModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_NearbyPlaces_Create, AppPermissions.Pages_Administration_NearbyPlaces_Edit)]
        public PartialViewResult PlaceLookupTableModal(long? id, string displayName)
        {
            var viewModel = new NearbyPlacePlaceLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_NearbyPlacePlaceLookupTableModal", viewModel);
        }

    }
}