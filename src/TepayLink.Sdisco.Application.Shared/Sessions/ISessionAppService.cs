using System.Threading.Tasks;
using Abp.Application.Services;
using TepayLink.Sdisco.Sessions.Dto;

namespace TepayLink.Sdisco.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
