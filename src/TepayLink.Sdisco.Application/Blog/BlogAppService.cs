using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;

namespace TepayLink.Sdisco.Blog
{
    public class BlogAppService : SdiscoAppServiceBase, IBlogAppService
    {

        private readonly IRepository<BlogPost, long> _blogPostRepository;

        private readonly IRepository<BlogComment, long> _blogCommentRepository;
        private readonly IRepository<Product, long> _tourrepository;
        private readonly IRepository<PartnerShip> _partnerShipRepository;

        private readonly IRepository<BlogProductRelated, long> _blogTourRelateRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserSubcriber, long> _userSubcriberRepository;
        private readonly ICommonAppService _commonAppService;

        public async Task<PagedResultDto<BasicBlogPostDto>> GetBlogPost(GetBlogPostInputDto input)
        {
            var query = _blogPostRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.Keyword), p =>
                (p.Title.Contains(input.Keyword) || p.Content.Contains(input.Keyword)) &&
                p.Status == BlogStatusEnum.Publish).OrderByDescending(p => p.CreationTime);
            var total = query.Count();
            var itemList = query.Select(p => new BasicBlogPostDto
            {
                Id = p.Id,
                Title = p.Title,
                ThumbImage = p.ThumbImage,
                PublishDate = p.PublishDate,
                ShortDesciption = p.ShortDescription,
            }).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var blogIds = itemList.Select(p => p.Id);
            var comments = _blogCommentRepository.GetAll().Where(p => blogIds.Contains(p.BlogPostId)).GroupBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Key,
                    Total = p.Count()
                }).ToList();

            foreach (var item in itemList)
            {
                var comment = comments.FirstOrDefault(p => p.Id == item.Id);
                item.TotalComment = comment != null ? comment.Total : 0;
            }

            return new PagedResultDto<BasicBlogPostDto>
            {
                TotalCount = total,
                Items = itemList
            };
        }

        public async Task<BlogDetailDto> GetBlogDetail(long blogId)
        {
            var blogDetail = _blogPostRepository.GetAll().Where(p => p.Id == blogId)
                .Select(p => new BlogDetailDto
                {
                    Content = p.Content,
                    Title = p.Title,
                    Id = p.Id,
                    ThumbImage = p.ThumbImage,
                    PublishDate = p.PublishDate
                }).FirstOrDefault();
            var totalComment = _blogCommentRepository.Count(p => p.BlogPostId == blogId);
            blogDetail.TotalComment = totalComment;
            return blogDetail;
        }

        public async Task<List<BasicBlogPostDto>> GetRecentPost(int limit)
        {
            var query = _blogPostRepository.GetAll().Where(p =>
                p.Status == BlogStatusEnum.Publish);

            var itemList = query.OrderByDescending(p => p.CreationTime).Select(p => new BasicBlogPostDto
            {
                Id = p.Id,
                Title = p.Title,
                ThumbImage = p.ThumbImage,
                PublishDate = p.PublishDate,
                ShortDesciption = p.ShortDescription,
            }).Take(limit).ToList();

            var blogIds = itemList.Select(p => p.Id);
            var comments = _blogCommentRepository.GetAll().Where(p => blogIds.Contains(p.BlogPostId)).GroupBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Key,
                    Total = p.Count()
                }).ToList();

            foreach (var item in itemList)
            {
                var comment = comments.FirstOrDefault(p => p.Id == item.Id);
                item.TotalComment = comment != null ? comment.Total : 0;
            }

            return itemList;
        }

        public async Task<List<BasicTourDto>> GetRelateTour(long blogId)
        {
            List<long> relateIds;
            if (blogId == 0)
            {
                relateIds = _tourrepository.GetAll().Where(p => p.Type == ProductTypeEnum.Tour && p.IsTop).Take(10).Select(p => p.Id).ToList();
            }
            else
            {
                relateIds = _blogTourRelateRepository.GetAll().Where(p => p.BlogPostId == blogId).Select(p => p.ProductId??0)
                  .ToList();

            }



            return await _commonAppService.GetTourByTourIds(relateIds);
        }

        public async Task<PagedResultDto<CommentOutputDto>> GetComment(GetCommentInputDto input)
        {
            var query = _blogCommentRepository.GetAll().Where(p => p.BlogPostId == input.BlogId)
                .OrderByDescending(p => p.CreationTime);
            var total = query.Count();
            var list = query.Select(p => new CommentOutputDto
            {
                Id = p.Id,
                BlogId = p.BlogPostId,
                Ratting = p.Rating,
                FullName = p.FullName,
                UserId = p.CreatorUserId ?? 0,
                Comment = p.Comment,
                CreatedDate = p.CreationTime
            }).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var ids = list.Select(p => p.Id).ToList();
            var userIds = list.Select(p => p.UserId).Distinct().ToList();
            var users = _userRepository.GetAll().Where(p => userIds.Contains(p.Id)).ToList();
            var replyCounts = _blogCommentRepository.GetAll()
                .Where(p => ids.Contains(p.ReplyId ?? 0))
                .GroupBy(p => p.ReplyId)
                .Select(p => new { p.Key, Total = p.Count() }).ToList();
            foreach (var item in list)
            {
                var user = users.FirstOrDefault(p => p.Id == item.UserId);
                if (user != null)
                {
                    item.Avatar = user.Avatar;
                    item.FullName = user.FullName;
                    item.Ratting = user.Rating;
                }
                var reply = replyCounts.FirstOrDefault(p => p.Key == item.Id);
                if (reply != null)
                {
                    item.ReplyCount = reply.Total;
                }
            }


            return new PagedResultDto<CommentOutputDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        public async Task<CommentOutputDto> AddComment(CommentInputDto input)
        {
            var comment = new BlogComment
            {
                FullName = input.FullName,
                Email = input.Email,
                Rating = input.Ratting,
                Comment = input.Comment,
                WebSite = input.Website,
                BlogPostId = input.BlogId
            };
            comment.Id = _blogCommentRepository.InsertAndGetId(comment);
            var user = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
            return new CommentOutputDto
            {

                FullName = input.FullName,
                Comment = input.Comment,
                BlogId = input.BlogId,
                Ratting = user != null ? user.Rating : 0,
                Avatar = user != null ? user.Avatar : "",
                CreatedDate = comment.CreationTime
            };
        }

        public async Task<List<PartnerShipDto>> GetPartnerShip()
        {
            var list = _partnerShipRepository.GetAll().OrderBy(p => p.Order).Select(p => new PartnerShipDto
            {
                Id = p.Id,
                Order = p.Order,
                Link = p.Link,
                Title = p.Title,
                Logo = p.Logo
            }).ToList();
            return list;
        }

        public async Task Subcribe(string email)
        {
            _userSubcriberRepository.Insert(new UserSubcriber
            {
                Email = email
            });
        }

        public async Task<PagedResultDto<CommentOutputDto>> GetReply(GetRepyInputDto input)
        {
            var query = _blogCommentRepository.GetAll().Where(p => p.ReplyId == input.CommentId)
               .OrderByDescending(p => p.CreationTime);
            var total = query.Count();
            var list = query.Select(p => new CommentOutputDto
            {
                Id = p.Id,
                BlogId = p.BlogPostId,
                Ratting = p.Rating,
                FullName = p.FullName,
                UserId = p.CreatorUserId ?? 0,
                Comment = p.Comment,
                CreatedDate = p.CreationTime
            }).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var ids = list.Select(p => p.Id).ToList();
            var userIds = list.Select(p => p.UserId).Distinct().ToList();
            var users = _userRepository.GetAll().Where(p => userIds.Contains(p.Id)).ToList();
            var replyCounts = _blogCommentRepository.GetAll()
                .Where(p => ids.Contains(p.ReplyId ?? 0))
                .GroupBy(p => p.ReplyId)
                .Select(p => new { p.Key, Total = p.Count() }).ToList();
            foreach (var item in list)
            {
                var user = users.FirstOrDefault(p => p.Id == item.UserId);
                if (user != null)
                {
                    item.Avatar = user.Avatar;
                    item.FullName = user.FullName;
                    item.Ratting = user.Rating;
                }
                var reply = replyCounts.FirstOrDefault(p => p.Key == item.Id);
                if (reply != null)
                {
                    item.ReplyCount = reply.Total;
                }
            }
            return new PagedResultDto<CommentOutputDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        public async Task<CommentOutputDto> ReplyComment(ReplyCommentInputDto input)
        {
            var comment = new BlogComment
            {
                FullName = input.FullName,
                Email = input.Email,
                Rating = input.Ratting,
                Comment = input.Comment,
                WebSite = input.Website,
                BlogPostId = input.BlogId,
                ReplyId = input.CommentId
            };
            comment.Id = _blogCommentRepository.InsertAndGetId(comment);
            var user = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
            return new CommentOutputDto
            {
                FullName = input.FullName,
                Comment = input.Comment,
                BlogId = input.BlogId,
                Ratting = user != null ? user.Rating : 0,
                Avatar = user != null ? user.Avatar : "",
                CreatedDate = comment.CreationTime
            };
        }

    }
}
