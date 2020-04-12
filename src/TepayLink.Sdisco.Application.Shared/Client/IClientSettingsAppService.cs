using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Client.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Client
{
    public interface IClientSettingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetClientSettingForViewDto>> GetAll(GetAllClientSettingsInput input);

        Task<GetClientSettingForViewDto> GetClientSettingForView(long id);

		Task<GetClientSettingForEditOutput> GetClientSettingForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditClientSettingDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetClientSettingsToExcel(GetAllClientSettingsForExcelInput input);

		
    }
}