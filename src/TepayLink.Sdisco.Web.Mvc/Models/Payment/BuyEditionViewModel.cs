using System.Collections.Generic;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.MultiTenancy.Payments;
using TepayLink.Sdisco.MultiTenancy.Payments.Dto;

namespace TepayLink.Sdisco.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
