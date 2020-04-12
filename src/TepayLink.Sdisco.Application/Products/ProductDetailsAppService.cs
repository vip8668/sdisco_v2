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
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductDetails)]
    public class ProductDetailsAppService : SdiscoAppServiceBase, IProductDetailsAppService
    {
		 private readonly IRepository<ProductDetail, long> _productDetailRepository;
		 private readonly IProductDetailsExcelExporter _productDetailsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public ProductDetailsAppService(IRepository<ProductDetail, long> productDetailRepository, IProductDetailsExcelExporter productDetailsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_productDetailRepository = productDetailRepository;
			_productDetailsExcelExporter = productDetailsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductDetailForViewDto>> GetAll(GetAllProductDetailsInput input)
         {
			
			var filteredProductDetails = _productDetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ThumbImage.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
						.WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredProductDetails = filteredProductDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productDetails = from o in pagedAndFilteredProductDetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductDetailForViewDto() {
							ProductDetail = new ProductDetailDto
							{
                                Title = o.Title,
                                Order = o.Order,
                                Description = o.Description,
                                ThumbImage = o.ThumbImage,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProductDetails.CountAsync();

            return new PagedResultDto<GetProductDetailForViewDto>(
                totalCount,
                await productDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetProductDetailForViewDto> GetProductDetailForView(long id)
         {
            var productDetail = await _productDetailRepository.GetAsync(id);

            var output = new GetProductDetailForViewDto { ProductDetail = ObjectMapper.Map<ProductDetailDto>(productDetail) };

		    if (output.ProductDetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetails_Edit)]
		 public async Task<GetProductDetailForEditOutput> GetProductDetailForEdit(EntityDto<long> input)
         {
            var productDetail = await _productDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductDetailForEditOutput {ProductDetail = ObjectMapper.Map<CreateOrEditProductDetailDto>(productDetail)};

		    if (output.ProductDetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetails_Create)]
		 protected virtual async Task Create(CreateOrEditProductDetailDto input)
         {
            var productDetail = ObjectMapper.Map<ProductDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				productDetail.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productDetailRepository.InsertAsync(productDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditProductDetailDto input)
         {
            var productDetail = await _productDetailRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetails_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductDetailsToExcel(GetAllProductDetailsForExcelInput input)
         {
			
			var filteredProductDetails = _productDetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ThumbImage.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
						.WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredProductDetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductDetailForViewDto() { 
							ProductDetail = new ProductDetailDto
							{
                                Title = o.Title,
                                Order = o.Order,
                                Description = o.Description,
                                ThumbImage = o.ThumbImage,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productDetailListDtos = await query.ToListAsync();

            return _productDetailsExcelExporter.ExportToFile(productDetailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_ProductDetails)]
         public async Task<PagedResultDto<ProductDetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductDetailProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductDetailProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductDetailProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}