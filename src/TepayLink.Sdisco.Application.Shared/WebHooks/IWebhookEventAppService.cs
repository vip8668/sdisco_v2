using System.Threading.Tasks;
using Abp.Webhooks;

namespace TepayLink.Sdisco.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
