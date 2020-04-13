namespace TepayLink.Sdisco.Booking.Dtos
{
    public class CheckCouponInputDto
    {
        public long BookingId { get; set; }
        /// <summary>
        /// Mã giảm giá
        /// </summary>
        public string CouponCode { get; set; }
    }
}