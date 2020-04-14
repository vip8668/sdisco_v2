using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Blog.Dtos;

namespace TepayLink.Sdisco.Blog
{
    public interface IBlogAppService : IApplicationService
    {
        Task<PagedResultDto<BasicBlogPostDto>> GetBlogPost(GetBlogPostInputDto input);
        Task<BlogDetailDto> GetBlogDetail(long blogId);
        Task<List<BasicBlogPostDto>> GetRecentPost(int limit);
        Task<List<BasicTourDto>> GetRelateTour(long blogId);
        Task<PagedResultDto<CommentOutputDto>> GetComment(GetCommentInputDto input);
        Task<PagedResultDto<CommentOutputDto>> GetReply(GetRepyInputDto input);
        Task<CommentOutputDto> AddComment(CommentInputDto input);
        Task<CommentOutputDto> ReplyComment(ReplyCommentInputDto input);

        Task<List<PartnerShipDto>> GetPartnerShip();
        Task Subcribe(string email);
    }
}
