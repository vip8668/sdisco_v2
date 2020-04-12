

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
	[AbpAuthorize(AppPermissions.Pages_Administration_UserSubcribers)]
    public class UserSubcribersAppService : SdiscoAppServiceBase, IUserSubcribersAppService
    {
		 private readonly IRepository<UserSubcriber, long> _userSubcriberRepository;
		 private readonly IUserSubcribersExcelExporter _userSubcribersExcelExporter;
		 

		  public UserSubcribersAppService(IRepository<UserSubcriber, long> userSubcriberRepository, IUserSubcribersExcelExporter userSubcribersExcelExporter ) 
		  {
			_userSubcriberRepository = userSubcriberRepository;
			_userSubcribersExcelExporter = userSubcribersExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetUserSubcriberForViewDto>> GetAll(GetAllUserSubcribersInput input)
         {
			
			var filteredUserSubcribers = _userSubcriberRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Email.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter);

			var pagedAndFilteredUserSubcribers = filteredUserSubcribers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userSubcribers = from o in pagedAndFilteredUserSubcribers
                         select new GetUserSubcriberForViewDto() {
							UserSubcriber = new UserSubcriberDto
							{
                                Email = o.Email,
                                Id = o.Id
							}
						};

            var totalCount = await filteredUserSubcribers.CountAsync();

            return new PagedResultDto<GetUserSubcriberForViewDto>(
                totalCount,
                await userSubcribers.ToListAsync()
            );
         }
		 
		 public async Task<GetUserSubcriberForViewDto> GetUserSubcriberForView(long id)
         {
            var userSubcriber = await _userSubcriberRepository.GetAsync(id);

            var output = new GetUserSubcriberForViewDto { UserSubcriber = ObjectMapper.Map<UserSubcriberDto>(userSubcriber) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_UserSubcribers_Edit)]
		 public async Task<GetUserSubcriberForEditOutput> GetUserSubcriberForEdit(EntityDto<long> input)
         {
            var userSubcriber = await _userSubcriberRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserSubcriberForEditOutput {UserSubcriber = ObjectMapper.Map<CreateOrEditUserSubcriberDto>(userSubcriber)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserSubcriberDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserSubcribers_Create)]
		 protected virtual async Task Create(CreateOrEditUserSubcriberDto input)
         {
            var userSubcriber = ObjectMapper.Map<UserSubcriber>(input);

			
			if (AbpSession.TenantId != null)
			{
				userSubcriber.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _userSubcriberRepository.InsertAsync(userSubcriber);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserSubcribers_Edit)]
		 protected virtual async Task Update(CreateOrEditUserSubcriberDto input)
         {
            var userSubcriber = await _userSubcriberRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, userSubcriber);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserSubcribers_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _userSubcriberRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserSubcribersToExcel(GetAllUserSubcribersForExcelInput input)
         {
			
			var filteredUserSubcribers = _userSubcriberRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Email.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter),  e => e.Email == input.EmailFilter);

			var query = (from o in filteredUserSubcribers
                         select new GetUserSubcriberForViewDto() { 
							UserSubcriber = new UserSubcriberDto
							{
                                Email = o.Email,
                                Id = o.Id
							}
						 });


            var userSubcriberListDtos = await query.ToListAsync();

            return _userSubcribersExcelExporter.ExportToFile(userSubcriberListDtos);
         }


    }
}