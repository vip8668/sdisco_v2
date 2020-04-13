

using TepayLink.Sdisco.AdminConfig;

namespace TepayLink.Sdisco.Payment.Dto
{
    public class PaymentInputDto
    {
        /// <summary>
        /// Id Đơn hàng
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// Id của phương thức thanh toán đã lưu ( nếu không chọn phương thức thanh toán đã lưu bỏ qua trường này)
        /// </summary>
        public long CardId { get; set; }
        /// <summary>
        /// Loại Thẻ  ( dùng khi thanh toán bằng thẻ mới chưa lưu)
        /// 1: Thẻ quốc tế
        /// 2: Thẻ nội địa
        /// 3: paypal ( Chưa hỗ trợ)
        /// </summary>
        public BankTypeEnum CardType { get; set; }
        
        /// <summary>
        /// Url khi thanh toán thành công
        /// </summary>
        public string SuccessUrl { get; set; }
        /// <summary>
        /// Url khi thanh toán thất bại
        /// </summary>
        public string FailUrl { get; set; }
    }
}