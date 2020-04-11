using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog
{
    public interface IBlogPostsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBlogPostForViewDto>> GetAll(GetAllBlogPostsInput input);

        Task<GetBlogPostForViewDto> GetBlogPostForView(long id);

		Task<GetBlogPostForEditOutput> GetBlogPostForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBlogPostDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBlogPostsToExcel(GetAllBlogPostsForExcelInput input);

		
    }
}