using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog
{
    public interface IBlogProductRelatedsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBlogProductRelatedForViewDto>> GetAll(GetAllBlogProductRelatedsInput input);

        Task<GetBlogProductRelatedForViewDto> GetBlogProductRelatedForView(long id);

		Task<GetBlogProductRelatedForEditOutput> GetBlogProductRelatedForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBlogProductRelatedDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBlogProductRelatedsToExcel(GetAllBlogProductRelatedsForExcelInput input);

		
		Task<PagedResultDto<BlogProductRelatedBlogPostLookupTableDto>> GetAllBlogPostForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BlogProductRelatedProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}