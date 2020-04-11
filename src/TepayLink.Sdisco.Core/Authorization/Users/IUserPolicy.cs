using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace TepayLink.Sdisco.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
