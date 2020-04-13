using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IRelatedProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRelatedProductForViewDto>> GetAll(GetAllRelatedProductsInput input);

        Task<GetRelatedProductForViewDto> GetRelatedProductForView(long id);

		Task<GetRelatedProductForEditOutput> GetRelatedProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRelatedProductDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetRelatedProductsToExcel(GetAllRelatedProductsForExcelInput input);

		
		Task<PagedResultDto<RelatedProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}