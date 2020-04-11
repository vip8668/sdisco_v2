using TepayLink.Sdisco.Editions.Dto;

namespace TepayLink.Sdisco.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < SdiscoConsts.MinimumUpgradePaymentAmount;
        }
    }
}
