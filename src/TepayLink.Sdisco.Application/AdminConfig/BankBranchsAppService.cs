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
	[AbpAuthorize(AppPermissions.Pages_BankBranchs)]
    public class BankBranchsAppService : SdiscoAppServiceBase, IBankBranchsAppService
    {
		 private readonly IRepository<BankBranch> _bankBranchRepository;
		 private readonly IBankBranchsExcelExporter _bankBranchsExcelExporter;
		 private readonly IRepository<Bank,int> _lookup_bankRepository;
		 

		  public BankBranchsAppService(IRepository<BankBranch> bankBranchRepository, IBankBranchsExcelExporter bankBranchsExcelExporter , IRepository<Bank, int> lookup_bankRepository) 
		  {
			_bankBranchRepository = bankBranchRepository;
			_bankBranchsExcelExporter = bankBranchsExcelExporter;
			_lookup_bankRepository = lookup_bankRepository;
		
		  }

		 public async Task<PagedResultDto<GetBankBranchForViewDto>> GetAll(GetAllBankBranchsInput input)
         {
			
			var filteredBankBranchs = _bankBranchRepository.GetAll()
						.Include( e => e.BankFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.BranchName.Contains(input.Filter) || e.Address.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter), e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter);

			var pagedAndFilteredBankBranchs = filteredBankBranchs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bankBranchs = from o in pagedAndFilteredBankBranchs
                         join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBankBranchForViewDto() {
							BankBranch = new BankBranchDto
							{
                                BranchName = o.BranchName,
                                Address = o.Address,
                                Order = o.Order,
                                Id = o.Id
							},
                         	BankBankName = s1 == null ? "" : s1.BankName.ToString()
						};

            var totalCount = await filteredBankBranchs.CountAsync();

            return new PagedResultDto<GetBankBranchForViewDto>(
                totalCount,
                await bankBranchs.ToListAsync()
            );
         }
		 
		 public async Task<GetBankBranchForViewDto> GetBankBranchForView(int id)
         {
            var bankBranch = await _bankBranchRepository.GetAsync(id);

            var output = new GetBankBranchForViewDto { BankBranch = ObjectMapper.Map<BankBranchDto>(bankBranch) };

		    if (output.BankBranch.BankId != null)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.BankBranch.BankId);
                output.BankBankName = _lookupBank.BankName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BankBranchs_Edit)]
		 public async Task<GetBankBranchForEditOutput> GetBankBranchForEdit(EntityDto input)
         {
            var bankBranch = await _bankBranchRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBankBranchForEditOutput {BankBranch = ObjectMapper.Map<CreateOrEditBankBranchDto>(bankBranch)};

		    if (output.BankBranch.BankId != null)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.BankBranch.BankId);
                output.BankBankName = _lookupBank.BankName.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBankBranchDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BankBranchs_Create)]
		 protected virtual async Task Create(CreateOrEditBankBranchDto input)
         {
            var bankBranch = ObjectMapper.Map<BankBranch>(input);

			
			if (AbpSession.TenantId != null)
			{
				bankBranch.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bankBranchRepository.InsertAsync(bankBranch);
         }

		 [AbpAuthorize(AppPermissions.Pages_BankBranchs_Edit)]
		 protected virtual async Task Update(CreateOrEditBankBranchDto input)
         {
            var bankBranch = await _bankBranchRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, bankBranch);
         }

		 [AbpAuthorize(AppPermissions.Pages_BankBranchs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bankBranchRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBankBranchsToExcel(GetAllBankBranchsForExcelInput input)
         {
			
			var filteredBankBranchs = _bankBranchRepository.GetAll()
						.Include( e => e.BankFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.BranchName.Contains(input.Filter) || e.Address.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter), e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter);

			var query = (from o in filteredBankBranchs
                         join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBankBranchForViewDto() { 
							BankBranch = new BankBranchDto
							{
                                BranchName = o.BranchName,
                                Address = o.Address,
                                Order = o.Order,
                                Id = o.Id
							},
                         	BankBankName = s1 == null ? "" : s1.BankName.ToString()
						 });


            var bankBranchListDtos = await query.ToListAsync();

            return _bankBranchsExcelExporter.ExportToFile(bankBranchListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_BankBranchs)]
         public async Task<PagedResultDto<BankBranchBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_bankRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.BankName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var bankList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BankBranchBankLookupTableDto>();
			foreach(var bank in bankList){
				lookupTableDtoList.Add(new BankBranchBankLookupTableDto
				{
					Id = bank.Id,
					DisplayName = bank.BankName?.ToString()
				});
			}

            return new PagedResultDto<BankBranchBankLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}