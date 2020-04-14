

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Clients.Exporting;
using TepayLink.Sdisco.Clients.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Clients
{
	[AbpAuthorize(AppPermissions.Pages_Administration_ClientSettings)]
    public class ClientSettingsAppService : SdiscoAppServiceBase, IClientSettingsAppService
    {
		 private readonly IRepository<ClientSetting, long> _clientSettingRepository;
		 private readonly IClientSettingsExcelExporter _clientSettingsExcelExporter;
		 

		  public ClientSettingsAppService(IRepository<ClientSetting, long> clientSettingRepository, IClientSettingsExcelExporter clientSettingsExcelExporter ) 
		  {
			_clientSettingRepository = clientSettingRepository;
			_clientSettingsExcelExporter = clientSettingsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetClientSettingForViewDto>> GetAll(GetAllClientSettingsInput input)
         {
			
			var filteredClientSettings = _clientSettingRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Key.Contains(input.Filter) || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyFilter),  e => e.Key == input.KeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var pagedAndFilteredClientSettings = filteredClientSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var clientSettings = from o in pagedAndFilteredClientSettings
                         select new GetClientSettingForViewDto() {
							ClientSetting = new ClientSettingDto
							{
                                Key = o.Key,
                                Value = o.Value,
                                Id = o.Id
							}
						};

            var totalCount = await filteredClientSettings.CountAsync();

            return new PagedResultDto<GetClientSettingForViewDto>(
                totalCount,
                await clientSettings.ToListAsync()
            );
         }
		 
		 public async Task<GetClientSettingForViewDto> GetClientSettingForView(long id)
         {
            var clientSetting = await _clientSettingRepository.GetAsync(id);

            var output = new GetClientSettingForViewDto { ClientSetting = ObjectMapper.Map<ClientSettingDto>(clientSetting) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ClientSettings_Edit)]
		 public async Task<GetClientSettingForEditOutput> GetClientSettingForEdit(EntityDto<long> input)
         {
            var clientSetting = await _clientSettingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetClientSettingForEditOutput {ClientSetting = ObjectMapper.Map<CreateOrEditClientSettingDto>(clientSetting)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditClientSettingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ClientSettings_Create)]
		 protected virtual async Task Create(CreateOrEditClientSettingDto input)
         {
            var clientSetting = ObjectMapper.Map<ClientSetting>(input);

			
			if (AbpSession.TenantId != null)
			{
				clientSetting.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _clientSettingRepository.InsertAsync(clientSetting);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ClientSettings_Edit)]
		 protected virtual async Task Update(CreateOrEditClientSettingDto input)
         {
            var clientSetting = await _clientSettingRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, clientSetting);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ClientSettings_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _clientSettingRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetClientSettingsToExcel(GetAllClientSettingsForExcelInput input)
         {
			
			var filteredClientSettings = _clientSettingRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Key.Contains(input.Filter) || e.Value.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeyFilter),  e => e.Key == input.KeyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter),  e => e.Value == input.ValueFilter);

			var query = (from o in filteredClientSettings
                         select new GetClientSettingForViewDto() { 
							ClientSetting = new ClientSettingDto
							{
                                Key = o.Key,
                                Value = o.Value,
                                Id = o.Id
							}
						 });


            var clientSettingListDtos = await query.ToListAsync();

            return _clientSettingsExcelExporter.ExportToFile(clientSettingListDtos);
         }


    }
}