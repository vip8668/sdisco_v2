using TepayLink.Sdisco.Products;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Products.Exporting;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Products
{
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails)]
    public class ProductReviewDetailsAppService : SdiscoAppServiceBase, IProductReviewDetailsAppService
    {
		 private readonly IRepository<ProductReviewDetail, long> _productReviewDetailRepository;
		 private readonly IProductReviewDetailsExcelExporter _productReviewDetailsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public ProductReviewDetailsAppService(IRepository<ProductReviewDetail, long> productReviewDetailRepository, IProductReviewDetailsExcelExporter productReviewDetailsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_productReviewDetailRepository = productReviewDetailRepository;
			_productReviewDetailsExcelExporter = productReviewDetailsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductReviewDetailForViewDto>> GetAll(GetAllProductReviewDetailsInput input)
         {
			
			var filteredProductReviewDetails = _productReviewDetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.ReplyComment.Contains(input.Filter) || e.Avatar.Contains(input.Filter) || e.Reviewer.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredProductReviewDetails = filteredProductReviewDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productReviewDetails = from o in pagedAndFilteredProductReviewDetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductReviewDetailForViewDto() {
							ProductReviewDetail = new ProductReviewDetailDto
							{
                                RatingAvg = o.RatingAvg,
                                Intineraty = o.Intineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Title = o.Title,
                                Comment = o.Comment,
                                BookingId = o.BookingId,
                                Read = o.Read,
                                ReplyComment = o.ReplyComment,
                                ReplyId = o.ReplyId,
                                Avatar = o.Avatar,
                                Reviewer = o.Reviewer,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProductReviewDetails.CountAsync();

            return new PagedResultDto<GetProductReviewDetailForViewDto>(
                totalCount,
                await productReviewDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetProductReviewDetailForViewDto> GetProductReviewDetailForView(long id)
         {
            var productReviewDetail = await _productReviewDetailRepository.GetAsync(id);

            var output = new GetProductReviewDetailForViewDto { ProductReviewDetail = ObjectMapper.Map<ProductReviewDetailDto>(productReviewDetail) };

		    if (output.ProductReviewDetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReviewDetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Edit)]
		 public async Task<GetProductReviewDetailForEditOutput> GetProductReviewDetailForEdit(EntityDto<long> input)
         {
            var productReviewDetail = await _productReviewDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductReviewDetailForEditOutput {ProductReviewDetail = ObjectMapper.Map<CreateOrEditProductReviewDetailDto>(productReviewDetail)};

		    if (output.ProductReviewDetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReviewDetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductReviewDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Create)]
		 protected virtual async Task Create(CreateOrEditProductReviewDetailDto input)
         {
            var productReviewDetail = ObjectMapper.Map<ProductReviewDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				productReviewDetail.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productReviewDetailRepository.InsertAsync(productReviewDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditProductReviewDetailDto input)
         {
            var productReviewDetail = await _productReviewDetailRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productReviewDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productReviewDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductReviewDetailsToExcel(GetAllProductReviewDetailsForExcelInput input)
         {
			
			var filteredProductReviewDetails = _productReviewDetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.ReplyComment.Contains(input.Filter) || e.Avatar.Contains(input.Filter) || e.Reviewer.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredProductReviewDetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductReviewDetailForViewDto() { 
							ProductReviewDetail = new ProductReviewDetailDto
							{
                                RatingAvg = o.RatingAvg,
                                Intineraty = o.Intineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Title = o.Title,
                                Comment = o.Comment,
                                BookingId = o.BookingId,
                                Read = o.Read,
                                ReplyComment = o.ReplyComment,
                                ReplyId = o.ReplyId,
                                Avatar = o.Avatar,
                                Reviewer = o.Reviewer,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productReviewDetailListDtos = await query.ToListAsync();

            return _productReviewDetailsExcelExporter.ExportToFile(productReviewDetailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails)]
         public async Task<PagedResultDto<ProductReviewDetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductReviewDetailProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductReviewDetailProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductReviewDetailProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}