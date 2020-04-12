using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductUtilitiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductUtilityForViewDto>> GetAll(GetAllProductUtilitiesInput input);

        Task<GetProductUtilityForViewDto> GetProductUtilityForView(long id);

		Task<GetProductUtilityForEditOutput> GetProductUtilityForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductUtilityDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductUtilitiesToExcel(GetAllProductUtilitiesForExcelInput input);

		
		Task<PagedResultDto<ProductUtilityProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProductUtilityUtilityLookupTableDto>> GetAllUtilityForLookupTable(GetAllForLookupTableInput input);
		
    }
}