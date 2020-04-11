
using TepayLink.Sdisco.Blog;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Blog.Exporting;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Blog
{
	[AbpAuthorize(AppPermissions.Pages_Administration_BlogPosts)]
    public class BlogPostsAppService : SdiscoAppServiceBase, IBlogPostsAppService
    {
		 private readonly IRepository<BlogPost, long> _blogPostRepository;
		 private readonly IBlogPostsExcelExporter _blogPostsExcelExporter;
		 

		  public BlogPostsAppService(IRepository<BlogPost, long> blogPostRepository, IBlogPostsExcelExporter blogPostsExcelExporter ) 
		  {
			_blogPostRepository = blogPostRepository;
			_blogPostsExcelExporter = blogPostsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetBlogPostForViewDto>> GetAll(GetAllBlogPostsInput input)
         {
			
			var filteredBlogPosts = _blogPostRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.Content.Contains(input.Filter) || e.ThumbImage.Contains(input.Filter));

			var pagedAndFilteredBlogPosts = filteredBlogPosts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var blogPosts = from o in pagedAndFilteredBlogPosts
                         select new GetBlogPostForViewDto() {
							BlogPost = new BlogPostDto
							{
                                Title = o.Title,
                                ShortDescription = o.ShortDescription,
                                Content = o.Content,
                                PublishDate = o.PublishDate,
                                ThumbImage = o.ThumbImage,
                                Status = o.Status,
                                Id = o.Id
							}
						};

            var totalCount = await filteredBlogPosts.CountAsync();

            return new PagedResultDto<GetBlogPostForViewDto>(
                totalCount,
                await blogPosts.ToListAsync()
            );
         }
		 
		 public async Task<GetBlogPostForViewDto> GetBlogPostForView(long id)
         {
            var blogPost = await _blogPostRepository.GetAsync(id);

            var output = new GetBlogPostForViewDto { BlogPost = ObjectMapper.Map<BlogPostDto>(blogPost) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogPosts_Edit)]
		 public async Task<GetBlogPostForEditOutput> GetBlogPostForEdit(EntityDto<long> input)
         {
            var blogPost = await _blogPostRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBlogPostForEditOutput {BlogPost = ObjectMapper.Map<CreateOrEditBlogPostDto>(blogPost)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBlogPostDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogPosts_Create)]
		 protected virtual async Task Create(CreateOrEditBlogPostDto input)
         {
            var blogPost = ObjectMapper.Map<BlogPost>(input);

			
			if (AbpSession.TenantId != null)
			{
				blogPost.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _blogPostRepository.InsertAsync(blogPost);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogPosts_Edit)]
		 protected virtual async Task Update(CreateOrEditBlogPostDto input)
         {
            var blogPost = await _blogPostRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, blogPost);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogPosts_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _blogPostRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBlogPostsToExcel(GetAllBlogPostsForExcelInput input)
         {
			
			var filteredBlogPosts = _blogPostRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.Content.Contains(input.Filter) || e.ThumbImage.Contains(input.Filter));

			var query = (from o in filteredBlogPosts
                         select new GetBlogPostForViewDto() { 
							BlogPost = new BlogPostDto
							{
                                Title = o.Title,
                                ShortDescription = o.ShortDescription,
                                Content = o.Content,
                                PublishDate = o.PublishDate,
                                ThumbImage = o.ThumbImage,
                                Status = o.Status,
                                Id = o.Id
							}
						 });


            var blogPostListDtos = await query.ToListAsync();

            return _blogPostsExcelExporter.ExportToFile(blogPostListDtos);
         }


    }
}