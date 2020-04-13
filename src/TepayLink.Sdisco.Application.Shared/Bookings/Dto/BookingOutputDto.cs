namespace TepayLink.Sdisco.Booking.Dtos
{
    public class BookingOutputDto
    {
        /// <summary>
        /// Id Booking
        /// </summary>
        public long BookingId { get; set; }
        /// <summary>
        /// Có thể thanh toán ngay hoặc phải chờ Admin xác nhận
        /// (true) có thể thanh toán ngay
        /// </summary>
        public bool CanpayNow { get; set; }
    }
}