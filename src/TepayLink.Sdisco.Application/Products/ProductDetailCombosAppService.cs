using TepayLink.Sdisco.Products;
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
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos)]
    public class ProductDetailCombosAppService : SdiscoAppServiceBase, IProductDetailCombosAppService
    {
		 private readonly IRepository<ProductDetailCombo, long> _productDetailComboRepository;
		 private readonly IProductDetailCombosExcelExporter _productDetailCombosExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 private readonly IRepository<ProductDetail,long> _lookup_productDetailRepository;
		 

		  public ProductDetailCombosAppService(IRepository<ProductDetailCombo, long> productDetailComboRepository, IProductDetailCombosExcelExporter productDetailCombosExcelExporter , IRepository<Product, long> lookup_productRepository, IRepository<ProductDetail, long> lookup_productDetailRepository) 
		  {
			_productDetailComboRepository = productDetailComboRepository;
			_productDetailCombosExcelExporter = productDetailCombosExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		_lookup_productDetailRepository = lookup_productDetailRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductDetailComboForViewDto>> GetAll(GetAllProductDetailCombosInput input)
         {
			
			var filteredProductDetailCombos = _productDetailComboRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.ProductDetailFk)
						.Include( e => e.ItemFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductDetailTitleFilter), e => e.ProductDetailFk != null && e.ProductDetailFk.Title == input.ProductDetailTitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.ItemFk != null && e.ItemFk.Name == input.ProductName2Filter);

			var pagedAndFilteredProductDetailCombos = filteredProductDetailCombos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productDetailCombos = from o in pagedAndFilteredProductDetailCombos
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productDetailRepository.GetAll() on o.ProductDetailId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_productRepository.GetAll() on o.ItemId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProductDetailComboForViewDto() {
							ProductDetailCombo = new ProductDetailComboDto
							{
                                RoomId = o.RoomId,
                                Description = o.Description,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductDetailTitle = s2 == null ? "" : s2.Title.ToString(),
                         	ProductName2 = s3 == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredProductDetailCombos.CountAsync();

            return new PagedResultDto<GetProductDetailComboForViewDto>(
                totalCount,
                await productDetailCombos.ToListAsync()
            );
         }
		 
		 public async Task<GetProductDetailComboForViewDto> GetProductDetailComboForView(long id)
         {
            var productDetailCombo = await _productDetailComboRepository.GetAsync(id);

            var output = new GetProductDetailComboForViewDto { ProductDetailCombo = ObjectMapper.Map<ProductDetailComboDto>(productDetailCombo) };

		    if (output.ProductDetailCombo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDetailCombo.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.ProductDetailCombo.ProductDetailId != null)
            {
                var _lookupProductDetail = await _lookup_productDetailRepository.FirstOrDefaultAsync((long)output.ProductDetailCombo.ProductDetailId);
                output.ProductDetailTitle = _lookupProductDetail.Title.ToString();
            }

		    if (output.ProductDetailCombo.ItemId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDetailCombo.ItemId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Edit)]
		 public async Task<GetProductDetailComboForEditOutput> GetProductDetailComboForEdit(EntityDto<long> input)
         {
            var productDetailCombo = await _productDetailComboRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductDetailComboForEditOutput {ProductDetailCombo = ObjectMapper.Map<CreateOrEditProductDetailComboDto>(productDetailCombo)};

		    if (output.ProductDetailCombo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDetailCombo.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }

		    if (output.ProductDetailCombo.ProductDetailId != null)
            {
                var _lookupProductDetail = await _lookup_productDetailRepository.FirstOrDefaultAsync((long)output.ProductDetailCombo.ProductDetailId);
                output.ProductDetailTitle = _lookupProductDetail.Title.ToString();
            }

		    if (output.ProductDetailCombo.ItemId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDetailCombo.ItemId);
                output.ProductName2 = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductDetailComboDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Create)]
		 protected virtual async Task Create(CreateOrEditProductDetailComboDto input)
         {
            var productDetailCombo = ObjectMapper.Map<ProductDetailCombo>(input);

			
			if (AbpSession.TenantId != null)
			{
				productDetailCombo.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productDetailComboRepository.InsertAsync(productDetailCombo);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Edit)]
		 protected virtual async Task Update(CreateOrEditProductDetailComboDto input)
         {
            var productDetailCombo = await _productDetailComboRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productDetailCombo);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productDetailComboRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductDetailCombosToExcel(GetAllProductDetailCombosForExcelInput input)
         {
			
			var filteredProductDetailCombos = _productDetailComboRepository.GetAll()
						.Include( e => e.ProductFk)
						.Include( e => e.ProductDetailFk)
						.Include( e => e.ItemFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductDetailTitleFilter), e => e.ProductDetailFk != null && e.ProductDetailFk.Title == input.ProductDetailTitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductName2Filter), e => e.ItemFk != null && e.ItemFk.Name == input.ProductName2Filter);

			var query = (from o in filteredProductDetailCombos
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_productDetailRepository.GetAll() on o.ProductDetailId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_productRepository.GetAll() on o.ItemId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetProductDetailComboForViewDto() { 
							ProductDetailCombo = new ProductDetailComboDto
							{
                                RoomId = o.RoomId,
                                Description = o.Description,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString(),
                         	ProductDetailTitle = s2 == null ? "" : s2.Title.ToString(),
                         	ProductName2 = s3 == null ? "" : s3.Name.ToString()
						 });


            var productDetailComboListDtos = await query.ToListAsync();

            return _productDetailCombosExcelExporter.ExportToFile(productDetailComboListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos)]
         public async Task<PagedResultDto<ProductDetailComboProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductDetailComboProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductDetailComboProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductDetailComboProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_ProductDetailCombos)]
         public async Task<PagedResultDto<ProductDetailComboProductDetailLookupTableDto>> GetAllProductDetailForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productDetailRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productDetailList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductDetailComboProductDetailLookupTableDto>();
			foreach(var productDetail in productDetailList){
				lookupTableDtoList.Add(new ProductDetailComboProductDetailLookupTableDto
				{
					Id = productDetail.Id,
					DisplayName = productDetail.Title?.ToString()
				});
			}

            return new PagedResultDto<ProductDetailComboProductDetailLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}