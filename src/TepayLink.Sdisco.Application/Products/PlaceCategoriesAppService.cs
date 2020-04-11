

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
	[AbpAuthorize(AppPermissions.Pages_Administration_PlaceCategories)]
    public class PlaceCategoriesAppService : SdiscoAppServiceBase, IPlaceCategoriesAppService
    {
		 private readonly IRepository<PlaceCategory> _placeCategoryRepository;
		 private readonly IPlaceCategoriesExcelExporter _placeCategoriesExcelExporter;
		 

		  public PlaceCategoriesAppService(IRepository<PlaceCategory> placeCategoryRepository, IPlaceCategoriesExcelExporter placeCategoriesExcelExporter ) 
		  {
			_placeCategoryRepository = placeCategoryRepository;
			_placeCategoriesExcelExporter = placeCategoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPlaceCategoryForViewDto>> GetAll(GetAllPlaceCategoriesInput input)
         {
			
			var filteredPlaceCategories = _placeCategoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Image.Contains(input.Filter) || e.Icon.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredPlaceCategories = filteredPlaceCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var placeCategories = from o in pagedAndFilteredPlaceCategories
                         select new GetPlaceCategoryForViewDto() {
							PlaceCategory = new PlaceCategoryDto
							{
                                Name = o.Name,
                                Image = o.Image,
                                Icon = o.Icon,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPlaceCategories.CountAsync();

            return new PagedResultDto<GetPlaceCategoryForViewDto>(
                totalCount,
                await placeCategories.ToListAsync()
            );
         }
		 
		 public async Task<GetPlaceCategoryForViewDto> GetPlaceCategoryForView(int id)
         {
            var placeCategory = await _placeCategoryRepository.GetAsync(id);

            var output = new GetPlaceCategoryForViewDto { PlaceCategory = ObjectMapper.Map<PlaceCategoryDto>(placeCategory) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_PlaceCategories_Edit)]
		 public async Task<GetPlaceCategoryForEditOutput> GetPlaceCategoryForEdit(EntityDto input)
         {
            var placeCategory = await _placeCategoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPlaceCategoryForEditOutput {PlaceCategory = ObjectMapper.Map<CreateOrEditPlaceCategoryDto>(placeCategory)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPlaceCategoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PlaceCategories_Create)]
		 protected virtual async Task Create(CreateOrEditPlaceCategoryDto input)
         {
            var placeCategory = ObjectMapper.Map<PlaceCategory>(input);

			
			if (AbpSession.TenantId != null)
			{
				placeCategory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _placeCategoryRepository.InsertAsync(placeCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PlaceCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditPlaceCategoryDto input)
         {
            var placeCategory = await _placeCategoryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, placeCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PlaceCategories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _placeCategoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPlaceCategoriesToExcel(GetAllPlaceCategoriesForExcelInput input)
         {
			
			var filteredPlaceCategories = _placeCategoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Image.Contains(input.Filter) || e.Icon.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredPlaceCategories
                         select new GetPlaceCategoryForViewDto() { 
							PlaceCategory = new PlaceCategoryDto
							{
                                Name = o.Name,
                                Image = o.Image,
                                Icon = o.Icon,
                                Id = o.Id
							}
						 });


            var placeCategoryListDtos = await query.ToListAsync();

            return _placeCategoriesExcelExporter.ExportToFile(placeCategoryListDtos);
         }


    }
}