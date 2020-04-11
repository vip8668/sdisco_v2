using System.Threading.Tasks;
using Abp.Application.Services;
using TepayLink.Sdisco.Configuration.Tenants.Dto;

namespace TepayLink.Sdisco.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
