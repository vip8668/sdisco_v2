using System.Threading.Tasks;
using Abp.Application.Services;
using TepayLink.Sdisco.MultiTenancy.Payments.Dto;
using TepayLink.Sdisco.MultiTenancy.Payments.Stripe.Dto;

namespace TepayLink.Sdisco.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}