using TepayLink.Sdisco.Products;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Account.Exporting;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Account
{
	[AbpAuthorize(AppPermissions.Pages_Administration_SaveItems)]
    public class SaveItemsAppService : SdiscoAppServiceBase, ISaveItemsAppService
    {
		 private readonly IRepository<SaveItem, long> _saveItemRepository;
		 private readonly ISaveItemsExcelExporter _saveItemsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public SaveItemsAppService(IRepository<SaveItem, long> saveItemRepository, ISaveItemsExcelExporter saveItemsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_saveItemRepository = saveItemRepository;
			_saveItemsExcelExporter = saveItemsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetSaveItemForViewDto>> GetAll(GetAllSaveItemsInput input)
         {
			
			var filteredSaveItems = _saveItemRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredSaveItems = filteredSaveItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var saveItems = from o in pagedAndFilteredSaveItems
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetSaveItemForViewDto() {
							SaveItem = new SaveItemDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredSaveItems.CountAsync();

            return new PagedResultDto<GetSaveItemForViewDto>(
                totalCount,
                await saveItems.ToListAsync()
            );
         }
		 
		 public async Task<GetSaveItemForViewDto> GetSaveItemForView(long id)
         {
            var saveItem = await _saveItemRepository.GetAsync(id);

            var output = new GetSaveItemForViewDto { SaveItem = ObjectMapper.Map<SaveItemDto>(saveItem) };

		    if (output.SaveItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SaveItem.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_SaveItems_Edit)]
		 public async Task<GetSaveItemForEditOutput> GetSaveItemForEdit(EntityDto<long> input)
         {
            var saveItem = await _saveItemRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSaveItemForEditOutput {SaveItem = ObjectMapper.Map<CreateOrEditSaveItemDto>(saveItem)};

		    if (output.SaveItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SaveItem.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSaveItemDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SaveItems_Create)]
		 protected virtual async Task Create(CreateOrEditSaveItemDto input)
         {
            var saveItem = ObjectMapper.Map<SaveItem>(input);

			
			if (AbpSession.TenantId != null)
			{
				saveItem.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _saveItemRepository.InsertAsync(saveItem);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SaveItems_Edit)]
		 protected virtual async Task Update(CreateOrEditSaveItemDto input)
         {
            var saveItem = await _saveItemRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, saveItem);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_SaveItems_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _saveItemRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSaveItemsToExcel(GetAllSaveItemsForExcelInput input)
         {
			
			var filteredSaveItems = _saveItemRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredSaveItems
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetSaveItemForViewDto() { 
							SaveItem = new SaveItemDto
							{
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var saveItemListDtos = await query.ToListAsync();

            return _saveItemsExcelExporter.ExportToFile(saveItemListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_SaveItems)]
         public async Task<PagedResultDto<SaveItemProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<SaveItemProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new SaveItemProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<SaveItemProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}