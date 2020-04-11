using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog
{
    public interface IBlogCommentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBlogCommentForViewDto>> GetAll(GetAllBlogCommentsInput input);

        Task<GetBlogCommentForViewDto> GetBlogCommentForView(long id);

		Task<GetBlogCommentForEditOutput> GetBlogCommentForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBlogCommentDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBlogCommentsToExcel(GetAllBlogCommentsForExcelInput input);

		
		Task<PagedResultDto<BlogCommentBlogPostLookupTableDto>> GetAllBlogPostForLookupTable(GetAllForLookupTableInput input);
		
    }
}