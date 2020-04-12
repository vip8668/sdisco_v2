using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(long id);

		Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

		
		Task<PagedResultDto<ProductCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProductUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProductPlaceLookupTableDto>> GetAllPlaceForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProductApplicationLanguageLookupTableDto>> GetAllApplicationLanguageForLookupTable(GetAllForLookupTableInput input);
		
    }
}