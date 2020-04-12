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
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities)]
    public class ProductUtilitiesAppService : SdiscoAppServiceBase, IProductUtilitiesAppService
    {
		 private readonly IRepository<ProductUtility, long> _productUtilityRepository;
		 private readonly IProductUtilitiesExcelExporter _productUtilitiesExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 private readonly IRepository<Utility,int> _lookup_utilityRepository;
		 

		  public ProductUtilitiesAppService(IRepository<ProductUtility, long> productUtilityRepository, IProductUtilitiesExcelExporter productUtilitiesExcelExporter , IRepository<Product, long> lookup_productRepository, IRepository<Utility, int> lookup_utilityRepository) 
		  {
			_productUtilityRepository = productUtilityRepository;
			_productUtilitiesExcelExporter = productUtilitiesExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		_lookup_utilityRepository = lookup_utilityRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductUtilityForViewDto>> GetAll(GetAllProductUtilitiesInput input)
         {
			
			var filteredProductUtilities = _productUtilityRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.UtilityFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UtilityNameFilter), e => e.UtilityFk != null && e.UtilityFk.Name == input.UtilityNameFilter);

			var pagedAndFilteredProductUtilities = filteredProductUtilities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productUtilities = from o in pagedAndFilteredProductUtilities
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_utilityRepository.GetAll() on o.UtilityId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetProductUtilityForViewDto() {
							ProductUtility = new ProductUtilityDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	UtilityName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredProductUtilities.CountAsync();

            return new PagedResultDto<GetProductUtilityForViewDto>(
                totalCount,
                await productUtilities.ToListAsync()
            );
         }
		 
		 public async Task<GetProductUtilityForViewDto> GetProductUtilityForView(long id)
         {
            var productUtility = await _productUtilityRepository.GetAsync(id);

            var output = new GetProductUtilityForViewDto { ProductUtility = ObjectMapper.Map<ProductUtilityDto>(productUtility) };

		    if (output.ProductUtility.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductUtility.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.ProductUtility.UtilityId != null)
            {
                var _lookupUtility = await _lookup_utilityRepository.FirstOrDefaultAsync((int)output.ProductUtility.UtilityId);
                output.UtilityName = _lookupUtility.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Edit)]
		 public async Task<GetProductUtilityForEditOutput> GetProductUtilityForEdit(EntityDto<long> input)
         {
            var productUtility = await _productUtilityRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductUtilityForEditOutput {ProductUtility = ObjectMapper.Map<CreateOrEditProductUtilityDto>(productUtility)};

		    if (output.ProductUtility.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductUtility.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.ProductUtility.UtilityId != null)
            {
                var _lookupUtility = await _lookup_utilityRepository.FirstOrDefaultAsync((int)output.ProductUtility.UtilityId);
                output.UtilityName = _lookupUtility.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductUtilityDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Create)]
		 protected virtual async Task Create(CreateOrEditProductUtilityDto input)
         {
            var productUtility = ObjectMapper.Map<ProductUtility>(input);

			
			if (AbpSession.TenantId != null)
			{
				productUtility.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productUtilityRepository.InsertAsync(productUtility);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Edit)]
		 protected virtual async Task Update(CreateOrEditProductUtilityDto input)
         {
            var productUtility = await _productUtilityRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productUtility);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productUtilityRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductUtilitiesToExcel(GetAllProductUtilitiesForExcelInput input)
         {
			
			var filteredProductUtilities = _productUtilityRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.UtilityFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UtilityNameFilter), e => e.UtilityFk != null && e.UtilityFk.Name == input.UtilityNameFilter);

			var query = (from o in filteredProductUtilities
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_utilityRepository.GetAll() on o.UtilityId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetProductUtilityForViewDto() { 
							ProductUtility = new ProductUtilityDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	UtilityName = s2 == null ? "" : s2.Name.ToString()
						 });


            var productUtilityListDtos = await query.ToListAsync();

            return _productUtilitiesExcelExporter.ExportToFile(productUtilityListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities)]
         public async Task<PagedResultDto<ProductUtilityProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductUtilityProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductUtilityProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductUtilityProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_ProductUtilities)]
         public async Task<PagedResultDto<ProductUtilityUtilityLookupTableDto>> GetAllUtilityForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_utilityRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var utilityList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductUtilityUtilityLookupTableDto>();
			foreach(var utility in utilityList){
				lookupTableDtoList.Add(new ProductUtilityUtilityLookupTableDto
				{
					Id = utility.Id,
					DisplayName = utility.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductUtilityUtilityLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}