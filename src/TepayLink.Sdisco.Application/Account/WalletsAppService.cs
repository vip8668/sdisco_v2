using TepayLink.Sdisco.Authorization.Users;

using TepayLink.Sdisco.Account;

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
	[AbpAuthorize(AppPermissions.Pages_Wallets)]
    public class WalletsAppService : SdiscoAppServiceBase, IWalletsAppService
    {
		 private readonly IRepository<Wallet, long> _walletRepository;
		 private readonly IWalletsExcelExporter _walletsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public WalletsAppService(IRepository<Wallet, long> walletRepository, IWalletsExcelExporter walletsExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_walletRepository = walletRepository;
			_walletsExcelExporter = walletsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetWalletForViewDto>> GetAll(GetAllWalletsInput input)
         {
			var typeFilter = (WalletTypeEnum) input.TypeFilter;
			
			var filteredWallets = _walletRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinBalanceFilter != null, e => e.Balance >= input.MinBalanceFilter)
						.WhereIf(input.MaxBalanceFilter != null, e => e.Balance <= input.MaxBalanceFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredWallets = filteredWallets
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var wallets = from o in pagedAndFilteredWallets
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetWalletForViewDto() {
							Wallet = new WalletDto
							{
                                Balance = o.Balance,
                                Type = o.Type,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredWallets.CountAsync();

            return new PagedResultDto<GetWalletForViewDto>(
                totalCount,
                await wallets.ToListAsync()
            );
         }
		 
		 public async Task<GetWalletForViewDto> GetWalletForView(long id)
         {
            var wallet = await _walletRepository.GetAsync(id);

            var output = new GetWalletForViewDto { Wallet = ObjectMapper.Map<WalletDto>(wallet) };

		    if (output.Wallet.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Wallet.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Wallets_Edit)]
		 public async Task<GetWalletForEditOutput> GetWalletForEdit(EntityDto<long> input)
         {
            var wallet = await _walletRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetWalletForEditOutput {Wallet = ObjectMapper.Map<CreateOrEditWalletDto>(wallet)};

		    if (output.Wallet.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Wallet.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditWalletDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Wallets_Create)]
		 protected virtual async Task Create(CreateOrEditWalletDto input)
         {
            var wallet = ObjectMapper.Map<Wallet>(input);

			
			if (AbpSession.TenantId != null)
			{
				wallet.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _walletRepository.InsertAsync(wallet);
         }

		 [AbpAuthorize(AppPermissions.Pages_Wallets_Edit)]
		 protected virtual async Task Update(CreateOrEditWalletDto input)
         {
            var wallet = await _walletRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, wallet);
         }

		 [AbpAuthorize(AppPermissions.Pages_Wallets_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _walletRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetWalletsToExcel(GetAllWalletsForExcelInput input)
         {
			var typeFilter = (WalletTypeEnum) input.TypeFilter;
			
			var filteredWallets = _walletRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinBalanceFilter != null, e => e.Balance >= input.MinBalanceFilter)
						.WhereIf(input.MaxBalanceFilter != null, e => e.Balance <= input.MaxBalanceFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredWallets
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetWalletForViewDto() { 
							Wallet = new WalletDto
							{
                                Balance = o.Balance,
                                Type = o.Type,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var walletListDtos = await query.ToListAsync();

            return _walletsExcelExporter.ExportToFile(walletListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Wallets)]
         public async Task<PagedResultDto<WalletUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<WalletUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new WalletUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<WalletUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}