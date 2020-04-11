using Abp.Application.Services.Dto;
using Abp.Webhooks;
using TepayLink.Sdisco.WebHooks.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
