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
	[AbpAuthorize(AppPermissions.Pages_Administration_Places)]
    public class PlacesAppService : SdiscoAppServiceBase, IPlacesAppService
    {
		 private readonly IRepository<Place, long> _placeRepository;
		 private readonly IPlacesExcelExporter _placesExcelExporter;
		 private readonly IRepository<Detination,long> _lookup_detinationRepository;
		 private readonly IRepository<PlaceCategory,int> _lookup_placeCategoryRepository;
		 

		  public PlacesAppService(IRepository<Place, long> placeRepository, IPlacesExcelExporter placesExcelExporter , IRepository<Detination, long> lookup_detinationRepository, IRepository<PlaceCategory, int> lookup_placeCategoryRepository) 
		  {
			_placeRepository = placeRepository;
			_placesExcelExporter = placesExcelExporter;
			_lookup_detinationRepository = lookup_detinationRepository;
		_lookup_placeCategoryRepository = lookup_placeCategoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetPlaceForViewDto>> GetAll(GetAllPlacesInput input)
         {
			
			var filteredPlaces = _placeRepository.GetAll()
						.Include( e => e.DetinationFk)
						.Include( e => e.PlaceCategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.DisplayAddress.Contains(input.Filter) || e.GoogleAddress.Contains(input.Filter) || e.Overview.Contains(input.Filter) || e.WhatToExpect.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DetinationNameFilter), e => e.DetinationFk != null && e.DetinationFk.Name == input.DetinationNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceCategoryNameFilter), e => e.PlaceCategoryFk != null && e.PlaceCategoryFk.Name == input.PlaceCategoryNameFilter);

			var pagedAndFilteredPlaces = filteredPlaces
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var places = from o in pagedAndFilteredPlaces
                         join o1 in _lookup_detinationRepository.GetAll() on o.DetinationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_placeCategoryRepository.GetAll() on o.PlaceCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetPlaceForViewDto() {
							Place = new PlaceDto
							{
                                Name = o.Name,
                                DisplayAddress = o.DisplayAddress,
                                GoogleAddress = o.GoogleAddress,
                                Overview = o.Overview,
                                WhatToExpect = o.WhatToExpect,
                                Id = o.Id
							},
                         	DetinationName = s1 == null ? "" : s1.Name.ToString(),
                         	PlaceCategoryName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredPlaces.CountAsync();

            return new PagedResultDto<GetPlaceForViewDto>(
                totalCount,
                await places.ToListAsync()
            );
         }
		 
		 public async Task<GetPlaceForViewDto> GetPlaceForView(long id)
         {
            var place = await _placeRepository.GetAsync(id);

            var output = new GetPlaceForViewDto { Place = ObjectMapper.Map<PlaceDto>(place) };

		    if (output.Place.DetinationId != null)
            {
                var _lookupDetination = await _lookup_detinationRepository.FirstOrDefaultAsync((long)output.Place.DetinationId);
                output.DetinationName = _lookupDetination.Name.ToString();
            }

		    if (output.Place.PlaceCategoryId != null)
            {
                var _lookupPlaceCategory = await _lookup_placeCategoryRepository.FirstOrDefaultAsync((int)output.Place.PlaceCategoryId);
                output.PlaceCategoryName = _lookupPlaceCategory.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Places_Edit)]
		 public async Task<GetPlaceForEditOutput> GetPlaceForEdit(EntityDto<long> input)
         {
            var place = await _placeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPlaceForEditOutput {Place = ObjectMapper.Map<CreateOrEditPlaceDto>(place)};

		    if (output.Place.DetinationId != null)
            {
                var _lookupDetination = await _lookup_detinationRepository.FirstOrDefaultAsync((long)output.Place.DetinationId);
                output.DetinationName = _lookupDetination.Name.ToString();
            }

		    if (output.Place.PlaceCategoryId != null)
            {
                var _lookupPlaceCategory = await _lookup_placeCategoryRepository.FirstOrDefaultAsync((int)output.Place.PlaceCategoryId);
                output.PlaceCategoryName = _lookupPlaceCategory.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPlaceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Places_Create)]
		 protected virtual async Task Create(CreateOrEditPlaceDto input)
         {
            var place = ObjectMapper.Map<Place>(input);

			
			if (AbpSession.TenantId != null)
			{
				place.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _placeRepository.InsertAsync(place);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Places_Edit)]
		 protected virtual async Task Update(CreateOrEditPlaceDto input)
         {
            var place = await _placeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, place);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Places_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _placeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPlacesToExcel(GetAllPlacesForExcelInput input)
         {
			
			var filteredPlaces = _placeRepository.GetAll()
						.Include( e => e.DetinationFk)
						.Include( e => e.PlaceCategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.DisplayAddress.Contains(input.Filter) || e.GoogleAddress.Contains(input.Filter) || e.Overview.Contains(input.Filter) || e.WhatToExpect.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DetinationNameFilter), e => e.DetinationFk != null && e.DetinationFk.Name == input.DetinationNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceCategoryNameFilter), e => e.PlaceCategoryFk != null && e.PlaceCategoryFk.Name == input.PlaceCategoryNameFilter);

			var query = (from o in filteredPlaces
                         join o1 in _lookup_detinationRepository.GetAll() on o.DetinationId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_placeCategoryRepository.GetAll() on o.PlaceCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetPlaceForViewDto() { 
							Place = new PlaceDto
							{
                                Name = o.Name,
                                DisplayAddress = o.DisplayAddress,
                                GoogleAddress = o.GoogleAddress,
                                Overview = o.Overview,
                                WhatToExpect = o.WhatToExpect,
                                Id = o.Id
							},
                         	DetinationName = s1 == null ? "" : s1.Name.ToString(),
                         	PlaceCategoryName = s2 == null ? "" : s2.Name.ToString()
						 });


            var placeListDtos = await query.ToListAsync();

            return _placesExcelExporter.ExportToFile(placeListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_Places)]
         public async Task<PagedResultDto<PlaceDetinationLookupTableDto>> GetAllDetinationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_detinationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var detinationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PlaceDetinationLookupTableDto>();
			foreach(var detination in detinationList){
				lookupTableDtoList.Add(new PlaceDetinationLookupTableDto
				{
					Id = detination.Id,
					DisplayName = detination.Name?.ToString()
				});
			}

            return new PagedResultDto<PlaceDetinationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_Places)]
         public async Task<PagedResultDto<PlacePlaceCategoryLookupTableDto>> GetAllPlaceCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_placeCategoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var placeCategoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PlacePlaceCategoryLookupTableDto>();
			foreach(var placeCategory in placeCategoryList){
				lookupTableDtoList.Add(new PlacePlaceCategoryLookupTableDto
				{
					Id = placeCategory.Id,
					DisplayName = placeCategory.Name?.ToString()
				});
			}

            return new PagedResultDto<PlacePlaceCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}