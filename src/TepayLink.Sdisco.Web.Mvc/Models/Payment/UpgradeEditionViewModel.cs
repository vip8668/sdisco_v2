using System.Collections.Generic;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.MultiTenancy.Payments;

namespace TepayLink.Sdisco.Web.Models.Payment
{
    public class UpgradeEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}