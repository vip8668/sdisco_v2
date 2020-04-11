
using TepayLink.Sdisco.AdminConfig;

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
	[AbpAuthorize(AppPermissions.Pages_Administration_Banks)]
    public class BanksAppService : SdiscoAppServiceBase, IBanksAppService
    {
		 private readonly IRepository<Bank> _bankRepository;
		 private readonly IBanksExcelExporter _banksExcelExporter;
		 

		  public BanksAppService(IRepository<Bank> bankRepository, IBanksExcelExporter banksExcelExporter ) 
		  {
			_bankRepository = bankRepository;
			_banksExcelExporter = banksExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetBankForViewDto>> GetAll(GetAllBanksInput input)
         {
			var typeFilter = (BankTypeEnum) input.TypeFilter;
			
			var filteredBanks = _bankRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.BankName.Contains(input.Filter) || e.BankCode.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Logo.Contains(input.Filter) || e.CardImage.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter),  e => e.BankName == input.BankNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankCodeFilter),  e => e.BankCode == input.BankCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter),  e => e.DisplayName == input.DisplayNameFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter);

			var pagedAndFilteredBanks = filteredBanks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var banks = from o in pagedAndFilteredBanks
                         select new GetBankForViewDto() {
							Bank = new BankDto
							{
                                BankName = o.BankName,
                                BankCode = o.BankCode,
                                DisplayName = o.DisplayName,
                                Type = o.Type,
                                Order = o.Order,
                                Logo = o.Logo,
                                CardImage = o.CardImage,
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredBanks.CountAsync();

            return new PagedResultDto<GetBankForViewDto>(
                totalCount,
                await banks.ToListAsync()
            );
         }
		 
		 public async Task<GetBankForViewDto> GetBankForView(int id)
         {
            var bank = await _bankRepository.GetAsync(id);

            var output = new GetBankForViewDto { Bank = ObjectMapper.Map<BankDto>(bank) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Banks_Edit)]
		 public async Task<GetBankForEditOutput> GetBankForEdit(EntityDto input)
         {
            var bank = await _bankRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBankForEditOutput {Bank = ObjectMapper.Map<CreateOrEditBankDto>(bank)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBankDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Banks_Create)]
		 protected virtual async Task Create(CreateOrEditBankDto input)
         {
            var bank = ObjectMapper.Map<Bank>(input);

			
			if (AbpSession.TenantId != null)
			{
				bank.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bankRepository.InsertAsync(bank);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Banks_Edit)]
		 protected virtual async Task Update(CreateOrEditBankDto input)
         {
            var bank = await _bankRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, bank);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Banks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bankRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBanksToExcel(GetAllBanksForExcelInput input)
         {
			var typeFilter = (BankTypeEnum) input.TypeFilter;
			
			var filteredBanks = _bankRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.BankName.Contains(input.Filter) || e.BankCode.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Logo.Contains(input.Filter) || e.CardImage.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter),  e => e.BankName == input.BankNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankCodeFilter),  e => e.BankCode == input.BankCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter),  e => e.DisplayName == input.DisplayNameFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter);

			var query = (from o in filteredBanks
                         select new GetBankForViewDto() { 
							Bank = new BankDto
							{
                                BankName = o.BankName,
                                BankCode = o.BankCode,
                                DisplayName = o.DisplayName,
                                Type = o.Type,
                                Order = o.Order,
                                Logo = o.Logo,
                                CardImage = o.CardImage,
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var bankListDtos = await query.ToListAsync();

            return _banksExcelExporter.ExportToFile(bankListDtos);
         }


    }
}