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
	[AbpAuthorize(AppPermissions.Pages_Administration_BlogComments)]
    public class BlogCommentsAppService : SdiscoAppServiceBase, IBlogCommentsAppService
    {
		 private readonly IRepository<BlogComment, long> _blogCommentRepository;
		 private readonly IBlogCommentsExcelExporter _blogCommentsExcelExporter;
		 private readonly IRepository<BlogPost,long> _lookup_blogPostRepository;
		 

		  public BlogCommentsAppService(IRepository<BlogComment, long> blogCommentRepository, IBlogCommentsExcelExporter blogCommentsExcelExporter , IRepository<BlogPost, long> lookup_blogPostRepository) 
		  {
			_blogCommentRepository = blogCommentRepository;
			_blogCommentsExcelExporter = blogCommentsExcelExporter;
			_lookup_blogPostRepository = lookup_blogPostRepository;
		
		  }

		 public async Task<PagedResultDto<GetBlogCommentForViewDto>> GetAll(GetAllBlogCommentsInput input)
         {
			
			var filteredBlogComments = _blogCommentRepository.GetAll()
						.Include( e => e.BlogPostFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Email.Contains(input.Filter) || e.FullName.Contains(input.Filter) || e.WebSite.Contains(input.Filter) || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FullNameFilter),  e => e.FullName == input.FullNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BlogPostTitleFilter), e => e.BlogPostFk != null && e.BlogPostFk.Title == input.BlogPostTitleFilter);

			var pagedAndFilteredBlogComments = filteredBlogComments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var blogComments = from o in pagedAndFilteredBlogComments
                         join o1 in _lookup_blogPostRepository.GetAll() on o.BlogPostId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBlogCommentForViewDto() {
							BlogComment = new BlogCommentDto
							{
                                Email = o.Email,
                                FullName = o.FullName,
                                Rating = o.Rating,
                                WebSite = o.WebSite,
                                Title = o.Title,
                                Comment = o.Comment,
                                Id = o.Id
							},
                         	BlogPostTitle = s1 == null ? "" : s1.Title.ToString()
						};

            var totalCount = await filteredBlogComments.CountAsync();

            return new PagedResultDto<GetBlogCommentForViewDto>(
                totalCount,
                await blogComments.ToListAsync()
            );
         }
		 
		 public async Task<GetBlogCommentForViewDto> GetBlogCommentForView(long id)
         {
            var blogComment = await _blogCommentRepository.GetAsync(id);

            var output = new GetBlogCommentForViewDto { BlogComment = ObjectMapper.Map<BlogCommentDto>(blogComment) };

		    if (output.BlogComment.BlogPostId != null)
            {
                var _lookupBlogPost = await _lookup_blogPostRepository.FirstOrDefaultAsync((long)output.BlogComment.BlogPostId);
                output.BlogPostTitle = _lookupBlogPost.Title.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogComments_Edit)]
		 public async Task<GetBlogCommentForEditOutput> GetBlogCommentForEdit(EntityDto<long> input)
         {
            var blogComment = await _blogCommentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBlogCommentForEditOutput {BlogComment = ObjectMapper.Map<CreateOrEditBlogCommentDto>(blogComment)};

		    if (output.BlogComment.BlogPostId != null)
            {
                var _lookupBlogPost = await _lookup_blogPostRepository.FirstOrDefaultAsync((long)output.BlogComment.BlogPostId);
                output.BlogPostTitle = _lookupBlogPost.Title.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBlogCommentDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogComments_Create)]
		 protected virtual async Task Create(CreateOrEditBlogCommentDto input)
         {
            var blogComment = ObjectMapper.Map<BlogComment>(input);

			
			if (AbpSession.TenantId != null)
			{
				blogComment.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _blogCommentRepository.InsertAsync(blogComment);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogComments_Edit)]
		 protected virtual async Task Update(CreateOrEditBlogCommentDto input)
         {
            var blogComment = await _blogCommentRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, blogComment);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BlogComments_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _blogCommentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBlogCommentsToExcel(GetAllBlogCommentsForExcelInput input)
         {
			
			var filteredBlogComments = _blogCommentRepository.GetAll()
						.Include( e => e.BlogPostFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Email.Contains(input.Filter) || e.FullName.Contains(input.Filter) || e.WebSite.Contains(input.Filter) || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FullNameFilter),  e => e.FullName == input.FullNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BlogPostTitleFilter), e => e.BlogPostFk != null && e.BlogPostFk.Title == input.BlogPostTitleFilter);

			var query = (from o in filteredBlogComments
                         join o1 in _lookup_blogPostRepository.GetAll() on o.BlogPostId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBlogCommentForViewDto() { 
							BlogComment = new BlogCommentDto
							{
                                Email = o.Email,
                                FullName = o.FullName,
                                Rating = o.Rating,
                                WebSite = o.WebSite,
                                Title = o.Title,
                                Comment = o.Comment,
                                Id = o.Id
							},
                         	BlogPostTitle = s1 == null ? "" : s1.Title.ToString()
						 });


            var blogCommentListDtos = await query.ToListAsync();

            return _blogCommentsExcelExporter.ExportToFile(blogCommentListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_BlogComments)]
         public async Task<PagedResultDto<BlogCommentBlogPostLookupTableDto>> GetAllBlogPostForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_blogPostRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var blogPostList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BlogCommentBlogPostLookupTableDto>();
			foreach(var blogPost in blogPostList){
				lookupTableDtoList.Add(new BlogCommentBlogPostLookupTableDto
				{
					Id = blogPost.Id,
					DisplayName = blogPost.Title?.ToString()
				});
			}

            return new PagedResultDto<BlogCommentBlogPostLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}