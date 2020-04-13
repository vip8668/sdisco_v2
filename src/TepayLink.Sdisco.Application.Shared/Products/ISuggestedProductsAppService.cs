using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface ISuggestedProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSuggestedProductForViewDto>> GetAll(GetAllSuggestedProductsInput input);

        Task<GetSuggestedProductForViewDto> GetSuggestedProductForView(long id);

		Task<GetSuggestedProductForEditOutput> GetSuggestedProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditSuggestedProductDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetSuggestedProductsToExcel(GetAllSuggestedProductsForExcelInput input);

		
		Task<PagedResultDto<SuggestedProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}