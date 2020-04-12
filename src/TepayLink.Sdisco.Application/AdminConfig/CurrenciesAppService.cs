

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
	[AbpAuthorize(AppPermissions.Pages_Administration_Currencies)]
    public class CurrenciesAppService : SdiscoAppServiceBase, ICurrenciesAppService
    {
		 private readonly IRepository<Currency> _currencyRepository;
		 private readonly ICurrenciesExcelExporter _currenciesExcelExporter;
		 

		  public CurrenciesAppService(IRepository<Currency> currencyRepository, ICurrenciesExcelExporter currenciesExcelExporter ) 
		  {
			_currencyRepository = currencyRepository;
			_currenciesExcelExporter = currenciesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrenciesInput input)
         {
			
			var filteredCurrencies = _currencyRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Icon.Contains(input.Filter) || e.CurrencySign.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter),  e => e.DisplayName == input.DisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter),  e => e.Icon == input.IconFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CurrencySignFilter),  e => e.CurrencySign == input.CurrencySignFilter)
						.WhereIf(input.IsDisabledFilter > -1,  e => (input.IsDisabledFilter == 1 && e.IsDisabled) || (input.IsDisabledFilter == 0 && !e.IsDisabled) );

			var pagedAndFilteredCurrencies = filteredCurrencies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var currencies = from o in pagedAndFilteredCurrencies
                         select new GetCurrencyForViewDto() {
							Currency = new CurrencyDto
							{
                                Name = o.Name,
                                DisplayName = o.DisplayName,
                                Icon = o.Icon,
                                CurrencySign = o.CurrencySign,
                                IsDisabled = o.IsDisabled,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCurrencies.CountAsync();

            return new PagedResultDto<GetCurrencyForViewDto>(
                totalCount,
                await currencies.ToListAsync()
            );
         }
		 
		 public async Task<GetCurrencyForViewDto> GetCurrencyForView(int id)
         {
            var currency = await _currencyRepository.GetAsync(id);

            var output = new GetCurrencyForViewDto { Currency = ObjectMapper.Map<CurrencyDto>(currency) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Currencies_Edit)]
		 public async Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto input)
         {
            var currency = await _currencyRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCurrencyForEditOutput {Currency = ObjectMapper.Map<CreateOrEditCurrencyDto>(currency)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCurrencyDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Currencies_Create)]
		 protected virtual async Task Create(CreateOrEditCurrencyDto input)
         {
            var currency = ObjectMapper.Map<Currency>(input);

			
			if (AbpSession.TenantId != null)
			{
				currency.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _currencyRepository.InsertAsync(currency);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Currencies_Edit)]
		 protected virtual async Task Update(CreateOrEditCurrencyDto input)
         {
            var currency = await _currencyRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, currency);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Currencies_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _currencyRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCurrenciesToExcel(GetAllCurrenciesForExcelInput input)
         {
			
			var filteredCurrencies = _currencyRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Icon.Contains(input.Filter) || e.CurrencySign.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter),  e => e.DisplayName == input.DisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter),  e => e.Icon == input.IconFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CurrencySignFilter),  e => e.CurrencySign == input.CurrencySignFilter)
						.WhereIf(input.IsDisabledFilter > -1,  e => (input.IsDisabledFilter == 1 && e.IsDisabled) || (input.IsDisabledFilter == 0 && !e.IsDisabled) );

			var query = (from o in filteredCurrencies
                         select new GetCurrencyForViewDto() { 
							Currency = new CurrencyDto
							{
                                Name = o.Name,
                                DisplayName = o.DisplayName,
                                Icon = o.Icon,
                                CurrencySign = o.CurrencySign,
                                IsDisabled = o.IsDisabled,
                                Id = o.Id
							}
						 });


            var currencyListDtos = await query.ToListAsync();

            return _currenciesExcelExporter.ExportToFile(currencyListDtos);
         }


    }
}