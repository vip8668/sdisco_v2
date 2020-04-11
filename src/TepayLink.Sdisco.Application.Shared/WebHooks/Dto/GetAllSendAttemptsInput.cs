using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
