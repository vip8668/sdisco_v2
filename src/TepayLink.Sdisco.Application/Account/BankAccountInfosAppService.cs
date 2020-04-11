using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Authorization.Users;


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
	[AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos)]
    public class BankAccountInfosAppService : SdiscoAppServiceBase, IBankAccountInfosAppService
    {
		 private readonly IRepository<BankAccountInfo, long> _bankAccountInfoRepository;
		 private readonly IBankAccountInfosExcelExporter _bankAccountInfosExcelExporter;
		 private readonly IRepository<Bank,int> _lookup_bankRepository;
		 private readonly IRepository<BankBranch,int> _lookup_bankBranchRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public BankAccountInfosAppService(IRepository<BankAccountInfo, long> bankAccountInfoRepository, IBankAccountInfosExcelExporter bankAccountInfosExcelExporter , IRepository<Bank, int> lookup_bankRepository, IRepository<BankBranch, int> lookup_bankBranchRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_bankAccountInfoRepository = bankAccountInfoRepository;
			_bankAccountInfosExcelExporter = bankAccountInfosExcelExporter;
			_lookup_bankRepository = lookup_bankRepository;
		_lookup_bankBranchRepository = lookup_bankBranchRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetBankAccountInfoForViewDto>> GetAll(GetAllBankAccountInfosInput input)
         {
			
			var filteredBankAccountInfos = _bankAccountInfoRepository.GetAll()
						.Include( e => e.BankFk)
						.Include( e => e.BankBranchFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AccountName.Contains(input.Filter) || e.AccountNo.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter),  e => e.AccountName == input.AccountNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountNoFilter),  e => e.AccountNo == input.AccountNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter), e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankBranchBranchNameFilter), e => e.BankBranchFk != null && e.BankBranchFk.BranchName == input.BankBranchBranchNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredBankAccountInfos = filteredBankAccountInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bankAccountInfos = from o in pagedAndFilteredBankAccountInfos
                         join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_bankBranchRepository.GetAll() on o.BankBranchId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetBankAccountInfoForViewDto() {
							BankAccountInfo = new BankAccountInfoDto
							{
                                AccountName = o.AccountName,
                                AccountNo = o.AccountNo,
                                Id = o.Id
							},
                         	BankBankName = s1 == null ? "" : s1.BankName.ToString(),
                         	BankBranchBranchName = s2 == null ? "" : s2.BranchName.ToString(),
                         	UserName = s3 == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredBankAccountInfos.CountAsync();

            return new PagedResultDto<GetBankAccountInfoForViewDto>(
                totalCount,
                await bankAccountInfos.ToListAsync()
            );
         }
		 
		 public async Task<GetBankAccountInfoForViewDto> GetBankAccountInfoForView(long id)
         {
            var bankAccountInfo = await _bankAccountInfoRepository.GetAsync(id);

            var output = new GetBankAccountInfoForViewDto { BankAccountInfo = ObjectMapper.Map<BankAccountInfoDto>(bankAccountInfo) };

		    if (output.BankAccountInfo.BankId != null)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.BankAccountInfo.BankId);
                output.BankBankName = _lookupBank.BankName.ToString();
            }

		    if (output.BankAccountInfo.BankBranchId != null)
            {
                var _lookupBankBranch = await _lookup_bankBranchRepository.FirstOrDefaultAsync((int)output.BankAccountInfo.BankBranchId);
                output.BankBranchBranchName = _lookupBankBranch.BranchName.ToString();
            }

		    if (output.BankAccountInfo.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BankAccountInfo.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Edit)]
		 public async Task<GetBankAccountInfoForEditOutput> GetBankAccountInfoForEdit(EntityDto<long> input)
         {
            var bankAccountInfo = await _bankAccountInfoRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBankAccountInfoForEditOutput {BankAccountInfo = ObjectMapper.Map<CreateOrEditBankAccountInfoDto>(bankAccountInfo)};

		    if (output.BankAccountInfo.BankId != null)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.BankAccountInfo.BankId);
                output.BankBankName = _lookupBank.BankName.ToString();
            }

		    if (output.BankAccountInfo.BankBranchId != null)
            {
                var _lookupBankBranch = await _lookup_bankBranchRepository.FirstOrDefaultAsync((int)output.BankAccountInfo.BankBranchId);
                output.BankBranchBranchName = _lookupBankBranch.BranchName.ToString();
            }

		    if (output.BankAccountInfo.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BankAccountInfo.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBankAccountInfoDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Create)]
		 protected virtual async Task Create(CreateOrEditBankAccountInfoDto input)
         {
            var bankAccountInfo = ObjectMapper.Map<BankAccountInfo>(input);

			
			if (AbpSession.TenantId != null)
			{
				bankAccountInfo.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bankAccountInfoRepository.InsertAsync(bankAccountInfo);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Edit)]
		 protected virtual async Task Update(CreateOrEditBankAccountInfoDto input)
         {
            var bankAccountInfo = await _bankAccountInfoRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, bankAccountInfo);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _bankAccountInfoRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBankAccountInfosToExcel(GetAllBankAccountInfosForExcelInput input)
         {
			
			var filteredBankAccountInfos = _bankAccountInfoRepository.GetAll()
						.Include( e => e.BankFk)
						.Include( e => e.BankBranchFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AccountName.Contains(input.Filter) || e.AccountNo.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter),  e => e.AccountName == input.AccountNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AccountNoFilter),  e => e.AccountNo == input.AccountNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter), e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BankBranchBranchNameFilter), e => e.BankBranchFk != null && e.BankBranchFk.BranchName == input.BankBranchBranchNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredBankAccountInfos
                         join o1 in _lookup_bankRepository.GetAll() on o.BankId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_bankBranchRepository.GetAll() on o.BankBranchId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetBankAccountInfoForViewDto() { 
							BankAccountInfo = new BankAccountInfoDto
							{
                                AccountName = o.AccountName,
                                AccountNo = o.AccountNo,
                                Id = o.Id
							},
                         	BankBankName = s1 == null ? "" : s1.BankName.ToString(),
                         	BankBranchBranchName = s2 == null ? "" : s2.BranchName.ToString(),
                         	UserName = s3 == null ? "" : s3.Name.ToString()
						 });


            var bankAccountInfoListDtos = await query.ToListAsync();

            return _bankAccountInfosExcelExporter.ExportToFile(bankAccountInfoListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos)]
         public async Task<PagedResultDto<BankAccountInfoBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_bankRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.BankName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var bankList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BankAccountInfoBankLookupTableDto>();
			foreach(var bank in bankList){
				lookupTableDtoList.Add(new BankAccountInfoBankLookupTableDto
				{
					Id = bank.Id,
					DisplayName = bank.BankName?.ToString()
				});
			}

            return new PagedResultDto<BankAccountInfoBankLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos)]
         public async Task<PagedResultDto<BankAccountInfoBankBranchLookupTableDto>> GetAllBankBranchForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_bankBranchRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.BranchName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var bankBranchList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BankAccountInfoBankBranchLookupTableDto>();
			foreach(var bankBranch in bankBranchList){
				lookupTableDtoList.Add(new BankAccountInfoBankBranchLookupTableDto
				{
					Id = bankBranch.Id,
					DisplayName = bankBranch.BranchName?.ToString()
				});
			}

            return new PagedResultDto<BankAccountInfoBankBranchLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_BankAccountInfos)]
         public async Task<PagedResultDto<BankAccountInfoUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BankAccountInfoUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new BankAccountInfoUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<BankAccountInfoUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}