using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IPlacesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPlaceForViewDto>> GetAll(GetAllPlacesInput input);

        Task<GetPlaceForViewDto> GetPlaceForView(long id);

		Task<GetPlaceForEditOutput> GetPlaceForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPlaceDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPlacesToExcel(GetAllPlacesForExcelInput input);

		
		Task<PagedResultDto<PlaceDetinationLookupTableDto>> GetAllDetinationForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PlacePlaceCategoryLookupTableDto>> GetAllPlaceCategoryForLookupTable(GetAllForLookupTableInput input);
		
    }
}