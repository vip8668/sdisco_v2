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
	[AbpAuthorize(AppPermissions.Pages_Administration_SuggestedProducts)]
    public class SuggestedProductsAppService : SdiscoAppServiceBase, ISuggestedProductsAppService
    {
		 private readonly IRepository<SuggestedProduct, long> _suggestedProductRepository;
		 private readonly ISuggestedProductsExcelExporter _suggestedProductsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public SuggestedProductsAppService(IRepository<SuggestedProduct, long> suggestedProductRepository, ISuggestedProductsExcelExporter suggestedProductsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_suggestedProductRepository = suggestedProductRepository;
			_suggestedProductsExcelExporter = suggestedProductsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetSuggestedProductForViewDto>> GetAll(GetAllSuggestedProductsInput input)
         {
			
			var filteredSuggestedProducts = _suggestedProductRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.SuggestedProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.SuggestedProductFk != null && e.SuggestedProductFk.Name == input.ProductName2Filter);

			var pagedAndFilteredSuggestedProducts = filteredSuggestedProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var suggestedProducts = from o in pagedAndFilteredSuggestedProducts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.SuggestedProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetSuggestedProductForViewDto() {
							SuggestedProduct = new SuggestedProductDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductName2 = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredSuggestedProducts.CountAsync();

            return new PagedResultDto<GetSuggestedProductForViewDto>(
                totalCount,
                await suggestedProducts.ToListAsync()
            );
         }
		 
		 public async Task<GetSuggestedProductForViewDto> GetSuggestedProductForView(long id)
         {
            var suggestedProduct = await _suggestedProductRepository.GetAsync(id);

            var output = new GetSuggestedProductForViewDto { SuggestedProduct = ObjectMapper.Map<SuggestedProductDto>(suggestedProduct) };

		    if (output.SuggestedProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SuggestedProduct.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.SuggestedProduct.SuggestedProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SuggestedProduct.SuggestedProductId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_SuggestedProducts_Edit)]
		 public async Task<GetSuggestedProductForEditOutput> GetSuggestedProductForEdit(EntityDto<long> input)
         {
            var suggestedProduct = await _suggestedProductRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSuggestedProductForEditOutput {SuggestedProduct = ObjectMapper.Map<CreateOrEditSuggestedProductDto>(suggestedProduct)};

		    if (output.SuggestedProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SuggestedProduct.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.SuggestedProduct.SuggestedProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SuggestedProduct.SuggestedProductId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSuggestedProductDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SuggestedProducts_Create)]
		 protected virtual async Task Create(CreateOrEditSuggestedProductDto input)
         {
            var suggestedProduct = ObjectMapper.Map<SuggestedProduct>(input);

			
			if (AbpSession.TenantId != null)
			{
				suggestedProduct.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _suggestedProductRepository.InsertAsync(suggestedProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SuggestedProducts_Edit)]
		 protected virtual async Task Update(CreateOrEditSuggestedProductDto input)
         {
            var suggestedProduct = await _suggestedProductRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, suggestedProduct);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SuggestedProducts_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _suggestedProductRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSuggestedProductsToExcel(GetAllSuggestedProductsForExcelInput input)
         {
			
			var filteredSuggestedProducts = _suggestedProductRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.SuggestedProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.SuggestedProductFk != null && e.SuggestedProductFk.Name == input.ProductName2Filter);

			var query = (from o in filteredSuggestedProducts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productRepository.GetAll() on o.SuggestedProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetSuggestedProductForViewDto() { 
							SuggestedProduct = new SuggestedProductDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductName2 = s2 == null ? "" : s2.Name.ToString()
						 });


            var suggestedProductListDtos = await query.ToListAsync();

            return _suggestedProductsExcelExporter.ExportToFile(suggestedProductListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_SuggestedProducts)]
         public async Task<PagedResultDto<SuggestedProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<SuggestedProductProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new SuggestedProductProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<SuggestedProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}