using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users;

namespace TepayLink.Sdisco.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
