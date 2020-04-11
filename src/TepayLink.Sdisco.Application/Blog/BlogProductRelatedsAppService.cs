using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Products;


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
	[AbpAuthorize(AppPermissions.Pages_BlogProductRelateds)]
    public class BlogProductRelatedsAppService : SdiscoAppServiceBase, IBlogProductRelatedsAppService
    {
		 private readonly IRepository<BlogProductRelated, long> _blogProductRelatedRepository;
		 private readonly IBlogProductRelatedsExcelExporter _blogProductRelatedsExcelExporter;
		 private readonly IRepository<BlogPost,long> _lookup_blogPostRepository;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public BlogProductRelatedsAppService(IRepository<BlogProductRelated, long> blogProductRelatedRepository, IBlogProductRelatedsExcelExporter blogProductRelatedsExcelExporter , IRepository<BlogPost, long> lookup_blogPostRepository, IRepository<Product, long> lookup_productRepository) 
		  {
			_blogProductRelatedRepository = blogProductRelatedRepository;
			_blogProductRelatedsExcelExporter = blogProductRelatedsExcelExporter;
			_lookup_blogPostRepository = lookup_blogPostRepository;
		_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetBlogProductRelatedForViewDto>> GetAll(GetAllBlogProductRelatedsInput input)
         {
			
			var filteredBlogProductRelateds = _blogProductRelatedRepository.GetAll()
						.Include( e => e.BlogPostFk)
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.BlogPostTitleFilter), e => e.BlogPostFk != null && e.BlogPostFk.Title == input.BlogPostTitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredBlogProductRelateds = filteredBlogProductRelateds
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var blogProductRelateds = from o in pagedAndFilteredBlogProductRelateds
                         join o1 in _lookup_blogPostRepository.GetAll() on o.BlogPostId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBlogProductRelatedForViewDto() {
							BlogProductRelated = new BlogProductRelatedDto
							{
                                Id = o.Id
							},
                         	BlogPostTitle = s1 == null ? "" : s1.Title.ToString(),
                         	ProductName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredBlogProductRelateds.CountAsync();

            return new PagedResultDto<GetBlogProductRelatedForViewDto>(
                totalCount,
                await blogProductRelateds.ToListAsync()
            );
         }
		 
		 public async Task<GetBlogProductRelatedForViewDto> GetBlogProductRelatedForView(long id)
         {
            var blogProductRelated = await _blogProductRelatedRepository.GetAsync(id);

            var output = new GetBlogProductRelatedForViewDto { BlogProductRelated = ObjectMapper.Map<BlogProductRelatedDto>(blogProductRelated) };

		    if (output.BlogProductRelated.BlogPostId != null)
            {
                var _lookupBlogPost = await _lookup_blogPostRepository.FirstOrDefaultAsync((long)output.BlogProductRelated.BlogPostId);
                output.BlogPostTitle = _lookupBlogPost.Title.ToString();
            }

		    if (output.BlogProductRelated.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.BlogProductRelated.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BlogProductRelateds_Edit)]
		 public async Task<GetBlogProductRelatedForEditOutput> GetBlogProductRelatedForEdit(EntityDto<long> input)
         {
            var blogProductRelated = await _blogProductRelatedRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBlogProductRelatedForEditOutput {BlogProductRelated = ObjectMapper.Map<CreateOrEditBlogProductRelatedDto>(blogProductRelated)};

		    if (output.BlogProductRelated.BlogPostId != null)
            {
                var _lookupBlogPost = await _lookup_blogPostRepository.FirstOrDefaultAsync((long)output.BlogProductRelated.BlogPostId);
                output.BlogPostTitle = _lookupBlogPost.Title.ToString();
            }

		    if (output.BlogProductRelated.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.BlogProductRelated.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBlogProductRelatedDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BlogProductRelateds_Create)]
		 protected virtual async Task Create(CreateOrEditBlogProductRelatedDto input)
         {
            var blogProductRelated = ObjectMapper.Map<BlogProductRelated>(input);

			
			if (AbpSession.TenantId != null)
			{
				blogProductRelated.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _blogProductRelatedRepository.InsertAsync(blogProductRelated);
         }

		 [AbpAuthorize(AppPermissions.Pages_BlogProductRelateds_Edit)]
		 protected virtual async Task Update(CreateOrEditBlogProductRelatedDto input)
         {
            var blogProductRelated = await _blogProductRelatedRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, blogProductRelated);
         }

		 [AbpAuthorize(AppPermissions.Pages_BlogProductRelateds_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _blogProductRelatedRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBlogProductRelatedsToExcel(GetAllBlogProductRelatedsForExcelInput input)
         {
			
			var filteredBlogProductRelateds = _blogProductRelatedRepository.GetAll()
						.Include( e => e.BlogPostFk)
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.BlogPostTitleFilter), e => e.BlogPostFk != null && e.BlogPostFk.Title == input.BlogPostTitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredBlogProductRelateds
                         join o1 in _lookup_blogPostRepository.GetAll() on o.BlogPostId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBlogProductRelatedForViewDto() { 
							BlogProductRelated = new BlogProductRelatedDto
							{
                                Id = o.Id
							},
                         	BlogPostTitle = s1 == null ? "" : s1.Title.ToString(),
                         	ProductName = s2 == null ? "" : s2.Name.ToString()
						 });


            var blogProductRelatedListDtos = await query.ToListAsync();

            return _blogProductRelatedsExcelExporter.ExportToFile(blogProductRelatedListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_BlogProductRelateds)]
         public async Task<PagedResultDto<BlogProductRelatedBlogPostLookupTableDto>> GetAllBlogPostForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_blogPostRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var blogPostList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BlogProductRelatedBlogPostLookupTableDto>();
			foreach(var blogPost in blogPostList){
				lookupTableDtoList.Add(new BlogProductRelatedBlogPostLookupTableDto
				{
					Id = blogPost.Id,
					DisplayName = blogPost.Title?.ToString()
				});
			}

            return new PagedResultDto<BlogProductRelatedBlogPostLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_BlogProductRelateds)]
         public async Task<PagedResultDto<BlogProductRelatedProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BlogProductRelatedProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new BlogProductRelatedProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<BlogProductRelatedProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}