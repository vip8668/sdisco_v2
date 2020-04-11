using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Common.Dto;
using TepayLink.Sdisco.Editions.Dto;

namespace TepayLink.Sdisco.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}