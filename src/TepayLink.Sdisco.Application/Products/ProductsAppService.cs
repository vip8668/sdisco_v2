using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using Abp.Localization;

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
	[AbpAuthorize(AppPermissions.Pages_Administration_Products)]
    public class ProductsAppService : SdiscoAppServiceBase, IProductsAppService
    {
		 private readonly IRepository<Product, long> _productRepository;
		 private readonly IProductsExcelExporter _productsExcelExporter;
		 private readonly IRepository<Category,int> _lookup_categoryRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Place,long> _lookup_placeRepository;
		 private readonly IRepository<ApplicationLanguage,int> _lookup_applicationLanguageRepository;
		 

		  public ProductsAppService(IRepository<Product, long> productRepository, IProductsExcelExporter productsExcelExporter , IRepository<Category, int> lookup_categoryRepository, IRepository<User, long> lookup_userRepository, IRepository<Place, long> lookup_placeRepository, IRepository<ApplicationLanguage, int> lookup_applicationLanguageRepository) 
		  {
			_productRepository = productRepository;
			_productsExcelExporter = productsExcelExporter;
			_lookup_categoryRepository = lookup_categoryRepository;
		_lookup_userRepository = lookup_userRepository;
		_lookup_placeRepository = lookup_placeRepository;
		_lookup_applicationLanguageRepository = lookup_applicationLanguageRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
         {
			var typeFilter = (ProductTypeEnum) input.TypeFilter;
			var statusFilter = (ProductStatusEnum) input.StatusFilter;
			
			var filteredProducts = _productRepository.GetAll()
						.Include( e => e.CategoryFk)
						.Include( e => e.HostUserFk)
						.Include( e => e.PlaceFk)
						.Include( e => e.LanguageFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Policies.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.FileName.Contains(input.Filter) || e.ExtraData.Contains(input.Filter) || e.WhatWeDo.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.IncludeTourGuideFilter > -1,  e => (input.IncludeTourGuideFilter == 1 && e.IncludeTourGuide) || (input.IncludeTourGuideFilter == 0 && !e.IncludeTourGuide) )
						.WhereIf(input.AllowRetailFilter > -1,  e => (input.AllowRetailFilter == 1 && e.AllowRetail) || (input.AllowRetailFilter == 0 && !e.AllowRetail) )
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.IsTrendingFilter > -1,  e => (input.IsTrendingFilter == 1 && e.IsTrending) || (input.IsTrendingFilter == 0 && !e.IsTrending) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExtraDataFilter),  e => e.ExtraData == input.ExtraDataFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WhatWeDoFilter),  e => e.WhatWeDo == input.WhatWeDoFilter)
						.WhereIf(input.MinLastBookTimeFilter != null, e => e.LastBookTime >= input.MinLastBookTimeFilter)
						.WhereIf(input.MaxLastBookTimeFilter != null, e => e.LastBookTime <= input.MaxLastBookTimeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.HostUserFk != null && e.HostUserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceNameFilter), e => e.PlaceFk != null && e.PlaceFk.Name == input.PlaceNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ApplicationLanguageNameFilter), e => e.LanguageFk != null && e.LanguageFk.Name == input.ApplicationLanguageNameFilter);

			var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var products = from o in pagedAndFilteredProducts
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.HostUserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_placeRepository.GetAll() on o.PlaceId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_applicationLanguageRepository.GetAll() on o.LanguageId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetProductForViewDto() {
							Product = new ProductDto
							{
                                Name = o.Name,
                                Type = o.Type,
                                Status = o.Status,
                                Description = o.Description,
                                Policies = o.Policies,
                                Duration = o.Duration,
                                StartTime = o.StartTime,
                                IncludeTourGuide = o.IncludeTourGuide,
                                AllowRetail = o.AllowRetail,
                                TotalSlot = o.TotalSlot,
                                Price = o.Price,
                                InstantBook = o.InstantBook,
                                TripLengh = o.TripLengh,
                                IsHotDeal = o.IsHotDeal,
                                IsBestSeller = o.IsBestSeller,
                                IsTrending = o.IsTrending,
                                IsTop = o.IsTop,
                                ExtraData = o.ExtraData,
                                WhatWeDo = o.WhatWeDo,
                                LastBookTime = o.LastBookTime,
                                Id = o.Id
							},
                         	CategoryName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString(),
                         	PlaceName = s3 == null ? "" : s3.Name.ToString(),
                         	ApplicationLanguageName = s4 == null ? "" : s4.Name.ToString()
						};

            var totalCount = await filteredProducts.CountAsync();

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                await products.ToListAsync()
            );
         }
		 
		 public async Task<GetProductForViewDto> GetProductForView(long id)
         {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

		    if (output.Product.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Product.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }

		    if (output.Product.HostUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Product.HostUserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Product.PlaceId != null)
            {
                var _lookupPlace = await _lookup_placeRepository.FirstOrDefaultAsync((long)output.Product.PlaceId);
                output.PlaceName = _lookupPlace.Name.ToString();
            }

		    if (output.Product.LanguageId != null)
            {
                var _lookupApplicationLanguage = await _lookup_applicationLanguageRepository.FirstOrDefaultAsync((int)output.Product.LanguageId);
                output.ApplicationLanguageName = _lookupApplicationLanguage.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Products_Edit)]
		 public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input)
         {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductForEditOutput {Product = ObjectMapper.Map<CreateOrEditProductDto>(product)};

		    if (output.Product.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.Product.CategoryId);
                output.CategoryName = _lookupCategory.Name.ToString();
            }

		    if (output.Product.HostUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Product.HostUserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Product.PlaceId != null)
            {
                var _lookupPlace = await _lookup_placeRepository.FirstOrDefaultAsync((long)output.Product.PlaceId);
                output.PlaceName = _lookupPlace.Name.ToString();
            }

		    if (output.Product.LanguageId != null)
            {
                var _lookupApplicationLanguage = await _lookup_applicationLanguageRepository.FirstOrDefaultAsync((int)output.Product.LanguageId);
                output.ApplicationLanguageName = _lookupApplicationLanguage.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Products_Create)]
		 protected virtual async Task Create(CreateOrEditProductDto input)
         {
            var product = ObjectMapper.Map<Product>(input);

			
			if (AbpSession.TenantId != null)
			{
				product.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productRepository.InsertAsync(product);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Products_Edit)]
		 protected virtual async Task Update(CreateOrEditProductDto input)
         {
            var product = await _productRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, product);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Products_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
         {
			var typeFilter = (ProductTypeEnum) input.TypeFilter;
			var statusFilter = (ProductStatusEnum) input.StatusFilter;
			
			var filteredProducts = _productRepository.GetAll()
						.Include( e => e.CategoryFk)
						.Include( e => e.HostUserFk)
						.Include( e => e.PlaceFk)
						.Include( e => e.LanguageFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Policies.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.FileName.Contains(input.Filter) || e.ExtraData.Contains(input.Filter) || e.WhatWeDo.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.IncludeTourGuideFilter > -1,  e => (input.IncludeTourGuideFilter == 1 && e.IncludeTourGuide) || (input.IncludeTourGuideFilter == 0 && !e.IncludeTourGuide) )
						.WhereIf(input.AllowRetailFilter > -1,  e => (input.AllowRetailFilter == 1 && e.AllowRetail) || (input.AllowRetailFilter == 0 && !e.AllowRetail) )
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.IsTrendingFilter > -1,  e => (input.IsTrendingFilter == 1 && e.IsTrending) || (input.IsTrendingFilter == 0 && !e.IsTrending) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExtraDataFilter),  e => e.ExtraData == input.ExtraDataFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WhatWeDoFilter),  e => e.WhatWeDo == input.WhatWeDoFilter)
						.WhereIf(input.MinLastBookTimeFilter != null, e => e.LastBookTime >= input.MinLastBookTimeFilter)
						.WhereIf(input.MaxLastBookTimeFilter != null, e => e.LastBookTime <= input.MaxLastBookTimeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.HostUserFk != null && e.HostUserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceNameFilter), e => e.PlaceFk != null && e.PlaceFk.Name == input.PlaceNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ApplicationLanguageNameFilter), e => e.LanguageFk != null && e.LanguageFk.Name == input.ApplicationLanguageNameFilter);

			var query = (from o in filteredProducts
                         join o1 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.HostUserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_placeRepository.GetAll() on o.PlaceId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_applicationLanguageRepository.GetAll() on o.LanguageId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetProductForViewDto() { 
							Product = new ProductDto
							{
                                Name = o.Name,
                                Type = o.Type,
                                Status = o.Status,
                                Description = o.Description,
                                Policies = o.Policies,
                                Duration = o.Duration,
                                StartTime = o.StartTime,
                                IncludeTourGuide = o.IncludeTourGuide,
                                AllowRetail = o.AllowRetail,
                                TotalSlot = o.TotalSlot,
                                Price = o.Price,
                                InstantBook = o.InstantBook,
                                TripLengh = o.TripLengh,
                                IsHotDeal = o.IsHotDeal,
                                IsBestSeller = o.IsBestSeller,
                                IsTrending = o.IsTrending,
                                IsTop = o.IsTop,
                                ExtraData = o.ExtraData,
                                WhatWeDo = o.WhatWeDo,
                                LastBookTime = o.LastBookTime,
                                Id = o.Id
							},
                         	CategoryName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString(),
                         	PlaceName = s3 == null ? "" : s3.Name.ToString(),
                         	ApplicationLanguageName = s4 == null ? "" : s4.Name.ToString()
						 });


            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_Products)]
         public async Task<PagedResultDto<ProductCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_categoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var categoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductCategoryLookupTableDto>();
			foreach(var category in categoryList){
				lookupTableDtoList.Add(new ProductCategoryLookupTableDto
				{
					Id = category.Id,
					DisplayName = category.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_Products)]
         public async Task<PagedResultDto<ProductUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ProductUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_Products)]
         public async Task<PagedResultDto<ProductPlaceLookupTableDto>> GetAllPlaceForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_placeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var placeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductPlaceLookupTableDto>();
			foreach(var place in placeList){
				lookupTableDtoList.Add(new ProductPlaceLookupTableDto
				{
					Id = place.Id,
					DisplayName = place.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductPlaceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_Products)]
         public async Task<PagedResultDto<ProductApplicationLanguageLookupTableDto>> GetAllApplicationLanguageForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_applicationLanguageRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var applicationLanguageList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductApplicationLanguageLookupTableDto>();
			foreach(var applicationLanguage in applicationLanguageList){
				lookupTableDtoList.Add(new ProductApplicationLanguageLookupTableDto
				{
					Id = applicationLanguage.Id,
					DisplayName = applicationLanguage.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductApplicationLanguageLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}