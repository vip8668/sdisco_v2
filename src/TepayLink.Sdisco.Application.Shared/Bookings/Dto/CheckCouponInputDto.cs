namespace TepayLink.Sdisco.Bookings.Dtos
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