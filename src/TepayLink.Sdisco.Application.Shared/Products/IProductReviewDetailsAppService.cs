using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductReviewDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductReviewDetailForViewDto>> GetAll(GetAllProductReviewDetailsInput input);

        Task<GetProductReviewDetailForViewDto> GetProductReviewDetailForView(long id);

		Task<GetProductReviewDetailForEditOutput> GetProductReviewDetailForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductReviewDetailDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductReviewDetailsToExcel(GetAllProductReviewDetailsForExcelInput input);

		
    }
}