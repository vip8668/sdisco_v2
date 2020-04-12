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
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductReviews)]
    public class ProductReviewsAppService : SdiscoAppServiceBase, IProductReviewsAppService
    {
		 private readonly IRepository<ProductReview, long> _productReviewRepository;
		 private readonly IProductReviewsExcelExporter _productReviewsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public ProductReviewsAppService(IRepository<ProductReview, long> productReviewRepository, IProductReviewsExcelExporter productReviewsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_productReviewRepository = productReviewRepository;
			_productReviewsExcelExporter = productReviewsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductReviewForViewDto>> GetAll(GetAllProductReviewsInput input)
         {
			
			var filteredProductReviews = _productReviewRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredProductReviews = filteredProductReviews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productReviews = from o in pagedAndFilteredProductReviews
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductReviewForViewDto() {
							ProductReview = new ProductReviewDto
							{
                                RatingAvg = o.RatingAvg,
                                ReviewCount = o.ReviewCount,
                                Intineraty = o.Intineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProductReviews.CountAsync();

            return new PagedResultDto<GetProductReviewForViewDto>(
                totalCount,
                await productReviews.ToListAsync()
            );
         }
		 
		 public async Task<GetProductReviewForViewDto> GetProductReviewForView(long id)
         {
            var productReview = await _productReviewRepository.GetAsync(id);

            var output = new GetProductReviewForViewDto { ProductReview = ObjectMapper.Map<ProductReviewDto>(productReview) };

		    if (output.ProductReview.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReview.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviews_Edit)]
		 public async Task<GetProductReviewForEditOutput> GetProductReviewForEdit(EntityDto<long> input)
         {
            var productReview = await _productReviewRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductReviewForEditOutput {ProductReview = ObjectMapper.Map<CreateOrEditProductReviewDto>(productReview)};

		    if (output.ProductReview.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReview.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductReviewDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviews_Create)]
		 protected virtual async Task Create(CreateOrEditProductReviewDto input)
         {
            var productReview = ObjectMapper.Map<ProductReview>(input);

			
			if (AbpSession.TenantId != null)
			{
				productReview.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productReviewRepository.InsertAsync(productReview);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviews_Edit)]
		 protected virtual async Task Update(CreateOrEditProductReviewDto input)
         {
            var productReview = await _productReviewRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productReview);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviews_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productReviewRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductReviewsToExcel(GetAllProductReviewsForExcelInput input)
         {
			
			var filteredProductReviews = _productReviewRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredProductReviews
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductReviewForViewDto() { 
							ProductReview = new ProductReviewDto
							{
                                RatingAvg = o.RatingAvg,
                                ReviewCount = o.ReviewCount,
                                Intineraty = o.Intineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productReviewListDtos = await query.ToListAsync();

            return _productReviewsExcelExporter.ExportToFile(productReviewListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_ProductReviews)]
         public async Task<PagedResultDto<ProductReviewProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductReviewProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductReviewProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductReviewProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}