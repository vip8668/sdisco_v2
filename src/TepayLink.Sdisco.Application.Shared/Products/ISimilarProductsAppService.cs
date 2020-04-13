using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface ISimilarProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSimilarProductForViewDto>> GetAll(GetAllSimilarProductsInput input);

        Task<GetSimilarProductForViewDto> GetSimilarProductForView(long id);

		Task<GetSimilarProductForEditOutput> GetSimilarProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditSimilarProductDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetSimilarProductsToExcel(GetAllSimilarProductsForExcelInput input);

		
		Task<PagedResultDto<SimilarProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}