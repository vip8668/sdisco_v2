using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductReviewsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductReviewForViewDto>> GetAll(GetAllProductReviewsInput input);

        Task<GetProductReviewForViewDto> GetProductReviewForView(long id);

		Task<GetProductReviewForEditOutput> GetProductReviewForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductReviewDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductReviewsToExcel(GetAllProductReviewsForExcelInput input);

		
		Task<PagedResultDto<ProductReviewProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}