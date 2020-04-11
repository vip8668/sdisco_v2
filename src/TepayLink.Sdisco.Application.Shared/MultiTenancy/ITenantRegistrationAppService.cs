using System.Threading.Tasks;
using Abp.Application.Services;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.MultiTenancy.Dto;

namespace TepayLink.Sdisco.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}