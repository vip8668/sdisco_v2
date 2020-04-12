

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.AdminConfig.Exporting;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.AdminConfig
{
	[AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : SdiscoAppServiceBase, ICountriesAppService
    {
		 private readonly IRepository<Country> _countryRepository;
		 private readonly ICountriesExcelExporter _countriesExcelExporter;
		 

		  public CountriesAppService(IRepository<Country> countryRepository, ICountriesExcelExporter countriesExcelExporter ) 
		  {
			_countryRepository = countryRepository;
			_countriesExcelExporter = countriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
         {
			
			var filteredCountries = _countryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Icon.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter),  e => e.DisplayName == input.DisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter),  e => e.Icon == input.IconFilter)
						.WhereIf(input.IsDisabledFilter > -1,  e => (input.IsDisabledFilter == 1 && e.IsDisabled) || (input.IsDisabledFilter == 0 && !e.IsDisabled) );

			var pagedAndFilteredCountries = filteredCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var countries = from o in pagedAndFilteredCountries
                         select new GetCountryForViewDto() {
							Country = new CountryDto
							{
                                Name = o.Name,
                                DisplayName = o.DisplayName,
                                Icon = o.Icon,
                                IsDisabled = o.IsDisabled,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCountries.CountAsync();

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                await countries.ToListAsync()
            );
         }
		 
		 public async Task<GetCountryForViewDto> GetCountryForView(int id)
         {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
		 public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input)
         {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCountryForEditOutput {Country = ObjectMapper.Map<CreateOrEditCountryDto>(country)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCountryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Create)]
		 protected virtual async Task Create(CreateOrEditCountryDto input)
         {
            var country = ObjectMapper.Map<Country>(input);

			
			if (AbpSession.TenantId != null)
			{
				country.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _countryRepository.InsertAsync(country);
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
		 protected virtual async Task Update(CreateOrEditCountryDto input)
         {
            var country = await _countryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, country);
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _countryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
         {
			
			var filteredCountries = _countryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Icon.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter),  e => e.DisplayName == input.DisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter),  e => e.Icon == input.IconFilter)
						.WhereIf(input.IsDisabledFilter > -1,  e => (input.IsDisabledFilter == 1 && e.IsDisabled) || (input.IsDisabledFilter == 0 && !e.IsDisabled) );

			var query = (from o in filteredCountries
                         select new GetCountryForViewDto() { 
							Country = new CountryDto
							{
                                Name = o.Name,
                                DisplayName = o.DisplayName,
                                Icon = o.Icon,
                                IsDisabled = o.IsDisabled,
                                Id = o.Id
							}
						 });


            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
         }


    }
}