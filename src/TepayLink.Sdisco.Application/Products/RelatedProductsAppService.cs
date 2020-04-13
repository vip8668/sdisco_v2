using TepayLink.Sdisco.Products;
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
	[AbpAuthorize(AppPermissions.Pages_Administration_RelatedProducts)]
    public class RelatedProductsAppService : SdiscoAppServiceBase, IRelatedProductsAppService
    {
		 private readonly IRepository<RelatedProduct, long> _relatedProductRepository;
		 private readonly IRelatedProductsExcelExporter _relatedProductsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public RelatedProductsAppService(IRepository<RelatedProduct, long> relatedProductRepository, IRelatedProductsExcelExporter relatedProductsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_relatedProductRepository = relatedProductRepository;
			_relatedProductsExcelExporter = relatedProductsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetRelatedProductForViewDto>> GetAll(GetAllRelatedProductsInput input)
         {
			
			var filteredRelatedProducts = _relatedProductRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.RelatedProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.RelatedProductFk != null && e.RelatedProductFk.Name == input.ProductName2Filter);

			var pagedAndFilteredRelatedProducts = filteredRelatedProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var relatedProducts = from o in pagedAndFilteredRelatedProducts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.RelatedProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRelatedProductForViewDto() {
							RelatedProduct = new RelatedProductDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductName2 = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredRelatedProducts.CountAsync();

            return new PagedResultDto<GetRelatedProductForViewDto>(
                totalCount,
                await relatedProducts.ToListAsync()
            );
         }
		 
		 public async Task<GetRelatedProductForViewDto> GetRelatedProductForView(long id)
         {
            var relatedProduct = await _relatedProductRepository.GetAsync(id);

            var output = new GetRelatedProductForViewDto { RelatedProduct = ObjectMapper.Map<RelatedProductDto>(relatedProduct) };

		    if (output.RelatedProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.RelatedProduct.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.RelatedProduct.RelatedProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.RelatedProduct.RelatedProductId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_RelatedProducts_Edit)]
		 public async Task<GetRelatedProductForEditOutput> GetRelatedProductForEdit(EntityDto<long> input)
         {
            var relatedProduct = await _relatedProductRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRelatedProductForEditOutput {RelatedProduct = ObjectMapper.Map<CreateOrEditRelatedProductDto>(relatedProduct)};

		    if (output.RelatedProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.RelatedProduct.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.RelatedProduct.RelatedProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.RelatedProduct.RelatedProductId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRelatedProductDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RelatedProducts_Create)]
		 protected virtual async Task Create(CreateOrEditRelatedProductDto input)
         {
            var relatedProduct = ObjectMapper.Map<RelatedProduct>(input);

			
			if (AbpSession.TenantId != null)
			{
				relatedProduct.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _relatedProductRepository.InsertAsync(relatedProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RelatedProducts_Edit)]
		 protected virtual async Task Update(CreateOrEditRelatedProductDto input)
         {
            var relatedProduct = await _relatedProductRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, relatedProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RelatedProducts_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _relatedProductRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetRelatedProductsToExcel(GetAllRelatedProductsForExcelInput input)
         {
			
			var filteredRelatedProducts = _relatedProductRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.RelatedProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.RelatedProductFk != null && e.RelatedProductFk.Name == input.ProductName2Filter);

			var query = (from o in filteredRelatedProducts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.RelatedProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRelatedProductForViewDto() { 
							RelatedProduct = new RelatedProductDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductName2 = s2 == null ? "" : s2.Name.ToString()
						 });


            var relatedProductListDtos = await query.ToListAsync();

            return _relatedProductsExcelExporter.ExportToFile(relatedProductListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_RelatedProducts)]
         public async Task<PagedResultDto<RelatedProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RelatedProductProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new RelatedProductProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<RelatedProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}