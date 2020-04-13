using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Places.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Places
{
    public interface INearbyPlacesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetNearbyPlaceForViewDto>> GetAll(GetAllNearbyPlacesInput input);

        Task<GetNearbyPlaceForViewDto> GetNearbyPlaceForView(long id);

		Task<GetNearbyPlaceForEditOutput> GetNearbyPlaceForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditNearbyPlaceDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetNearbyPlacesToExcel(GetAllNearbyPlacesForExcelInput input);

		
		Task<PagedResultDto<NearbyPlacePlaceLookupTableDto>> GetAllPlaceForLookupTable(GetAllForLookupTableInput input);
		
    }
}