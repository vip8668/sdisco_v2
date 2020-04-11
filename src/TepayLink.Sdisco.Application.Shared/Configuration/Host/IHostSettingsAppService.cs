using System.Threading.Tasks;
using Abp.Application.Services;
using TepayLink.Sdisco.Configuration.Host.Dto;

namespace TepayLink.Sdisco.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
