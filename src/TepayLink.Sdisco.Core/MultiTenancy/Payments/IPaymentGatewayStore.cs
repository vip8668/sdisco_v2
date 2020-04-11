using System.Collections.Generic;

namespace TepayLink.Sdisco.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
