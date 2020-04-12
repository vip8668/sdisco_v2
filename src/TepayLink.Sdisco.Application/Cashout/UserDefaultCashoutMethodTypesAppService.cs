using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Cashout.Exporting;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Cashout
{
	[AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes)]
    public class UserDefaultCashoutMethodTypesAppService : SdiscoAppServiceBase, IUserDefaultCashoutMethodTypesAppService
    {
		 private readonly IRepository<UserDefaultCashoutMethodType, long> _userDefaultCashoutMethodTypeRepository;
		 private readonly IUserDefaultCashoutMethodTypesExcelExporter _userDefaultCashoutMethodTypesExcelExporter;
		 private readonly IRepository<CashoutMethodType,int> _lookup_cashoutMethodTypeRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public UserDefaultCashoutMethodTypesAppService(IRepository<UserDefaultCashoutMethodType, long> userDefaultCashoutMethodTypeRepository, IUserDefaultCashoutMethodTypesExcelExporter userDefaultCashoutMethodTypesExcelExporter , IRepository<CashoutMethodType, int> lookup_cashoutMethodTypeRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_userDefaultCashoutMethodTypeRepository = userDefaultCashoutMethodTypeRepository;
			_userDefaultCashoutMethodTypesExcelExporter = userDefaultCashoutMethodTypesExcelExporter;
			_lookup_cashoutMethodTypeRepository = lookup_cashoutMethodTypeRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetUserDefaultCashoutMethodTypeForViewDto>> GetAll(GetAllUserDefaultCashoutMethodTypesInput input)
         {
			
			var filteredUserDefaultCashoutMethodTypes = _userDefaultCashoutMethodTypeRepository.GetAll()
						.Include( e => e.CashoutMethodTypeFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CashoutMethodTypeTitleFilter), e => e.CashoutMethodTypeFk != null && e.CashoutMethodTypeFk.Title == input.CashoutMethodTypeTitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredUserDefaultCashoutMethodTypes = filteredUserDefaultCashoutMethodTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userDefaultCashoutMethodTypes = from o in pagedAndFilteredUserDefaultCashoutMethodTypes
                         join o1 in _lookup_cashoutMethodTypeRepository.GetAll() on o.CashoutMethodTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserDefaultCashoutMethodTypeForViewDto() {
							UserDefaultCashoutMethodType = new UserDefaultCashoutMethodTypeDto
							{
                                Id = o.Id
							},
                         	CashoutMethodTypeTitle = s1 == null ? "" : s1.Title.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredUserDefaultCashoutMethodTypes.CountAsync();

            return new PagedResultDto<GetUserDefaultCashoutMethodTypeForViewDto>(
                totalCount,
                await userDefaultCashoutMethodTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetUserDefaultCashoutMethodTypeForViewDto> GetUserDefaultCashoutMethodTypeForView(long id)
         {
            var userDefaultCashoutMethodType = await _userDefaultCashoutMethodTypeRepository.GetAsync(id);

            var output = new GetUserDefaultCashoutMethodTypeForViewDto { UserDefaultCashoutMethodType = ObjectMapper.Map<UserDefaultCashoutMethodTypeDto>(userDefaultCashoutMethodType) };

		    if (output.UserDefaultCashoutMethodType.CashoutMethodTypeId != null)
            {
                var _lookupCashoutMethodType = await _lookup_cashoutMethodTypeRepository.FirstOrDefaultAsync((int)output.UserDefaultCashoutMethodType.CashoutMethodTypeId);
                output.CashoutMethodTypeTitle = _lookupCashoutMethodType.Title.ToString();
            }

		    if (output.UserDefaultCashoutMethodType.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDefaultCashoutMethodType.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Edit)]
		 public async Task<GetUserDefaultCashoutMethodTypeForEditOutput> GetUserDefaultCashoutMethodTypeForEdit(EntityDto<long> input)
         {
            var userDefaultCashoutMethodType = await _userDefaultCashoutMethodTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserDefaultCashoutMethodTypeForEditOutput {UserDefaultCashoutMethodType = ObjectMapper.Map<CreateOrEditUserDefaultCashoutMethodTypeDto>(userDefaultCashoutMethodType)};

		    if (output.UserDefaultCashoutMethodType.CashoutMethodTypeId != null)
            {
                var _lookupCashoutMethodType = await _lookup_cashoutMethodTypeRepository.FirstOrDefaultAsync((int)output.UserDefaultCashoutMethodType.CashoutMethodTypeId);
                output.CashoutMethodTypeTitle = _lookupCashoutMethodType.Title.ToString();
            }

		    if (output.UserDefaultCashoutMethodType.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDefaultCashoutMethodType.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserDefaultCashoutMethodTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Create)]
		 protected virtual async Task Create(CreateOrEditUserDefaultCashoutMethodTypeDto input)
         {
            var userDefaultCashoutMethodType = ObjectMapper.Map<UserDefaultCashoutMethodType>(input);

			
			if (AbpSession.TenantId != null)
			{
				userDefaultCashoutMethodType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _userDefaultCashoutMethodTypeRepository.InsertAsync(userDefaultCashoutMethodType);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditUserDefaultCashoutMethodTypeDto input)
         {
            var userDefaultCashoutMethodType = await _userDefaultCashoutMethodTypeRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, userDefaultCashoutMethodType);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _userDefaultCashoutMethodTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserDefaultCashoutMethodTypesToExcel(GetAllUserDefaultCashoutMethodTypesForExcelInput input)
         {
			
			var filteredUserDefaultCashoutMethodTypes = _userDefaultCashoutMethodTypeRepository.GetAll()
						.Include( e => e.CashoutMethodTypeFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CashoutMethodTypeTitleFilter), e => e.CashoutMethodTypeFk != null && e.CashoutMethodTypeFk.Title == input.CashoutMethodTypeTitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredUserDefaultCashoutMethodTypes
                         join o1 in _lookup_cashoutMethodTypeRepository.GetAll() on o.CashoutMethodTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserDefaultCashoutMethodTypeForViewDto() { 
							UserDefaultCashoutMethodType = new UserDefaultCashoutMethodTypeDto
							{
                                Id = o.Id
							},
                         	CashoutMethodTypeTitle = s1 == null ? "" : s1.Title.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						 });


            var userDefaultCashoutMethodTypeListDtos = await query.ToListAsync();

            return _userDefaultCashoutMethodTypesExcelExporter.ExportToFile(userDefaultCashoutMethodTypeListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes)]
         public async Task<PagedResultDto<UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableDto>> GetAllCashoutMethodTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_cashoutMethodTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var cashoutMethodTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableDto>();
			foreach(var cashoutMethodType in cashoutMethodTypeList){
				lookupTableDtoList.Add(new UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableDto
				{
					Id = cashoutMethodType.Id,
					DisplayName = cashoutMethodType.Title?.ToString()
				});
			}

            return new PagedResultDto<UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes)]
         public async Task<PagedResultDto<UserDefaultCashoutMethodTypeUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserDefaultCashoutMethodTypeUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserDefaultCashoutMethodTypeUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<UserDefaultCashoutMethodTypeUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}