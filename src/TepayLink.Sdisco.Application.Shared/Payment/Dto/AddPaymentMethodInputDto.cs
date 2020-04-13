

using TepayLink.Sdisco.AdminConfig;

namespace TepayLink.Sdisco.Payment.Dto
{
    public class AddPaymentMethodInputDto
    {
        /// <summary>
        /// Loại thẻ : 1 thẻ quốc thế, 2 Thẻ nội địa, 3 PayPal ( hiện chưa hỗ trợ paypal)
        /// </summary>
        public BankTypeEnum CardType { get; set; }

        public string SuccessUrl { get; set; }
        public string FailUrl { get; set; }
    }
}