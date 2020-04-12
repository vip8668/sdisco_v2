using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductDetailForViewDto>> GetAll(GetAllProductDetailsInput input);

        Task<GetProductDetailForViewDto> GetProductDetailForView(long id);

		Task<GetProductDetailForEditOutput> GetProductDetailForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductDetailDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductDetailsToExcel(GetAllProductDetailsForExcelInput input);

		
		Task<PagedResultDto<ProductDetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}