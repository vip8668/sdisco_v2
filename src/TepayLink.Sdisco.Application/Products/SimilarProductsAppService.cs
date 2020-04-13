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
	[AbpAuthorize(AppPermissions.Pages_Administration_SimilarProducts)]
    public class SimilarProductsAppService : SdiscoAppServiceBase, ISimilarProductsAppService
    {
		 private readonly IRepository<SimilarProduct, long> _similarProductRepository;
		 private readonly ISimilarProductsExcelExporter _similarProductsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public SimilarProductsAppService(IRepository<SimilarProduct, long> similarProductRepository, ISimilarProductsExcelExporter similarProductsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_similarProductRepository = similarProductRepository;
			_similarProductsExcelExporter = similarProductsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetSimilarProductForViewDto>> GetAll(GetAllSimilarProductsInput input)
         {
			
			var filteredSimilarProducts = _similarProductRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.SimilarProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.SimilarProductFk != null && e.SimilarProductFk.Name == input.ProductName2Filter);

			var pagedAndFilteredSimilarProducts = filteredSimilarProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var similarProducts = from o in pagedAndFilteredSimilarProducts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.SimilarProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetSimilarProductForViewDto() {
							SimilarProduct = new SimilarProductDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductName2 = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredSimilarProducts.CountAsync();

            return new PagedResultDto<GetSimilarProductForViewDto>(
                totalCount,
                await similarProducts.ToListAsync()
            );
         }
		 
		 public async Task<GetSimilarProductForViewDto> GetSimilarProductForView(long id)
         {
            var similarProduct = await _similarProductRepository.GetAsync(id);

            var output = new GetSimilarProductForViewDto { SimilarProduct = ObjectMapper.Map<SimilarProductDto>(similarProduct) };

		    if (output.SimilarProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SimilarProduct.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.SimilarProduct.SimilarProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SimilarProduct.SimilarProductId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_SimilarProducts_Edit)]
		 public async Task<GetSimilarProductForEditOutput> GetSimilarProductForEdit(EntityDto<long> input)
         {
            var similarProduct = await _similarProductRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSimilarProductForEditOutput {SimilarProduct = ObjectMapper.Map<CreateOrEditSimilarProductDto>(similarProduct)};

		    if (output.SimilarProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SimilarProduct.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.SimilarProduct.SimilarProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SimilarProduct.SimilarProductId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSimilarProductDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SimilarProducts_Create)]
		 protected virtual async Task Create(CreateOrEditSimilarProductDto input)
         {
            var similarProduct = ObjectMapper.Map<SimilarProduct>(input);

			
			if (AbpSession.TenantId != null)
			{
				similarProduct.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _similarProductRepository.InsertAsync(similarProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SimilarProducts_Edit)]
		 protected virtual async Task Update(CreateOrEditSimilarProductDto input)
         {
            var similarProduct = await _similarProductRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, similarProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SimilarProducts_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _similarProductRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSimilarProductsToExcel(GetAllSimilarProductsForExcelInput input)
         {
			
			var filteredSimilarProducts = _similarProductRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.SimilarProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.SimilarProductFk != null && e.SimilarProductFk.Name == input.ProductName2Filter);

			var query = (from o in filteredSimilarProducts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.SimilarProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetSimilarProductForViewDto() { 
							SimilarProduct = new SimilarProductDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductName2 = s2 == null ? "" : s2.Name.ToString()
						 });


            var similarProductListDtos = await query.ToListAsync();

            return _similarProductsExcelExporter.ExportToFile(similarProductListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_SimilarProducts)]
         public async Task<PagedResultDto<SimilarProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<SimilarProductProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new SimilarProductProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<SimilarProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}