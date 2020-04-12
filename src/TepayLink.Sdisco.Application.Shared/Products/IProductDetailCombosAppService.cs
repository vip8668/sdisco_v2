using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductDetailCombosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductDetailComboForViewDto>> GetAll(GetAllProductDetailCombosInput input);

        Task<GetProductDetailComboForViewDto> GetProductDetailComboForView(long id);

		Task<GetProductDetailComboForEditOutput> GetProductDetailComboForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductDetailComboDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductDetailCombosToExcel(GetAllProductDetailCombosForExcelInput input);

		
		Task<PagedResultDto<ProductDetailComboProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProductDetailComboProductDetailLookupTableDto>> GetAllProductDetailForLookupTable(GetAllForLookupTableInput input);
		
    }
}