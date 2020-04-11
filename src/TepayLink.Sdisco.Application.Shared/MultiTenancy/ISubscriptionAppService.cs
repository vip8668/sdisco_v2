using System.Threading.Tasks;
using Abp.Application.Services;

namespace TepayLink.Sdisco.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
