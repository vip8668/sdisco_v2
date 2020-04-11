using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Timing.Dto;

namespace TepayLink.Sdisco.Timing
{
    public interface ITimingAppService : IApplicationService
    {
        Task<ListResultDto<NameValueDto>> GetTimezones(GetTimezonesInput input);

        Task<List<ComboboxItemDto>> GetTimezoneComboboxItems(GetTimezoneComboboxItemsInput input);
    }
}
