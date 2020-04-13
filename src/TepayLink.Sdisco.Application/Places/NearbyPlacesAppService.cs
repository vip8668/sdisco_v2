using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Places.Exporting;
using TepayLink.Sdisco.Places.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Places
{
	[AbpAuthorize(AppPermissions.Pages_Administration_NearbyPlaces)]
    public class NearbyPlacesAppService : SdiscoAppServiceBase, INearbyPlacesAppService
    {
		 private readonly IRepository<NearbyPlace, long> _nearbyPlaceRepository;
		 private readonly INearbyPlacesExcelExporter _nearbyPlacesExcelExporter;
		 private readonly IRepository<Place,long> _lookup_placeRepository;
		 

		  public NearbyPlacesAppService(IRepository<NearbyPlace, long> nearbyPlaceRepository, INearbyPlacesExcelExporter nearbyPlacesExcelExporter , IRepository<Place, long> lookup_placeRepository) 
		  {
			_nearbyPlaceRepository = nearbyPlaceRepository;
			_nearbyPlacesExcelExporter = nearbyPlacesExcelExporter;
			_lookup_placeRepository = lookup_placeRepository;
		
		  }

		 public async Task<PagedResultDto<GetNearbyPlaceForViewDto>> GetAll(GetAllNearbyPlacesInput input)
         {
			
			var filteredNearbyPlaces = _nearbyPlaceRepository.GetAll()
						.Include( e => e.PlaceFk)
						.Include( e => e.NearbyPlaceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceNameFilter), e => e.PlaceFk != null && e.PlaceFk.Name == input.PlaceNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceName2Filter), e => e.NearbyPlaceFk != null && e.NearbyPlaceFk.Name == input.PlaceName2Filter);

			var pagedAndFilteredNearbyPlaces = filteredNearbyPlaces
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var nearbyPlaces = from o in pagedAndFilteredNearbyPlaces
                         join o1 in _lookup_placeRepository.GetAll() on o.PlaceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_placeRepository.GetAll() on o.NearbyPlaceId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetNearbyPlaceForViewDto() {
							NearbyPlace = new NearbyPlaceDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	PlaceName = s1 == null ? "" : s1.Name.ToString(),
                         	PlaceName2 = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredNearbyPlaces.CountAsync();

            return new PagedResultDto<GetNearbyPlaceForViewDto>(
                totalCount,
                await nearbyPlaces.ToListAsync()
            );
         }
		 
		 public async Task<GetNearbyPlaceForViewDto> GetNearbyPlaceForView(long id)
         {
            var nearbyPlace = await _nearbyPlaceRepository.GetAsync(id);

            var output = new GetNearbyPlaceForViewDto { NearbyPlace = ObjectMapper.Map<NearbyPlaceDto>(nearbyPlace) };

		    if (output.NearbyPlace.PlaceId != null)
            {
                var _lookupPlace = await _lookup_placeRepository.FirstOrDefaultAsync((long)output.NearbyPlace.PlaceId);
                output.PlaceName = _lookupPlace.Name.ToString();
            }

		    if (output.NearbyPlace.NearbyPlaceId != null)
            {
                var _lookupPlace = await _lookup_placeRepository.FirstOrDefaultAsync((long)output.NearbyPlace.NearbyPlaceId);
                output.PlaceName2 = _lookupPlace.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_NearbyPlaces_Edit)]
		 public async Task<GetNearbyPlaceForEditOutput> GetNearbyPlaceForEdit(EntityDto<long> input)
         {
            var nearbyPlace = await _nearbyPlaceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetNearbyPlaceForEditOutput {NearbyPlace = ObjectMapper.Map<CreateOrEditNearbyPlaceDto>(nearbyPlace)};

		    if (output.NearbyPlace.PlaceId != null)
            {
                var _lookupPlace = await _lookup_placeRepository.FirstOrDefaultAsync((long)output.NearbyPlace.PlaceId);
                output.PlaceName = _lookupPlace.Name.ToString();
            }

		    if (output.NearbyPlace.NearbyPlaceId != null)
            {
                var _lookupPlace = await _lookup_placeRepository.FirstOrDefaultAsync((long)output.NearbyPlace.NearbyPlaceId);
                output.PlaceName2 = _lookupPlace.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditNearbyPlaceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_NearbyPlaces_Create)]
		 protected virtual async Task Create(CreateOrEditNearbyPlaceDto input)
         {
            var nearbyPlace = ObjectMapper.Map<NearbyPlace>(input);

			
			if (AbpSession.TenantId != null)
			{
				nearbyPlace.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _nearbyPlaceRepository.InsertAsync(nearbyPlace);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_NearbyPlaces_Edit)]
		 protected virtual async Task Update(CreateOrEditNearbyPlaceDto input)
         {
            var nearbyPlace = await _nearbyPlaceRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, nearbyPlace);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_NearbyPlaces_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _nearbyPlaceRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetNearbyPlacesToExcel(GetAllNearbyPlacesForExcelInput input)
         {
			
			var filteredNearbyPlaces = _nearbyPlaceRepository.GetAll()
						.Include( e => e.PlaceFk)
						.Include( e => e.NearbyPlaceFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceNameFilter), e => e.PlaceFk != null && e.PlaceFk.Name == input.PlaceNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PlaceName2Filter), e => e.NearbyPlaceFk != null && e.NearbyPlaceFk.Name == input.PlaceName2Filter);

			var query = (from o in filteredNearbyPlaces
                         join o1 in _lookup_placeRepository.GetAll() on o.PlaceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_placeRepository.GetAll() on o.NearbyPlaceId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetNearbyPlaceForViewDto() { 
							NearbyPlace = new NearbyPlaceDto
							{
                                Description = o.Description,
                                Id = o.Id
							},
                         	PlaceName = s1 == null ? "" : s1.Name.ToString(),
                         	PlaceName2 = s2 == null ? "" : s2.Name.ToString()
						 });


            var nearbyPlaceListDtos = await query.ToListAsync();

            return _nearbyPlacesExcelExporter.ExportToFile(nearbyPlaceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_NearbyPlaces)]
         public async Task<PagedResultDto<NearbyPlacePlaceLookupTableDto>> GetAllPlaceForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_placeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var placeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<NearbyPlacePlaceLookupTableDto>();
			foreach(var place in placeList){
				lookupTableDtoList.Add(new NearbyPlacePlaceLookupTableDto
				{
					Id = place.Id,
					DisplayName = place.Name?.ToString()
				});
			}

            return new PagedResultDto<NearbyPlacePlaceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}