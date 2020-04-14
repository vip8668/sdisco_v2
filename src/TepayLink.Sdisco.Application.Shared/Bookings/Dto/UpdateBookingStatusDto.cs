

using TepayLink.Sdisco.Bookings;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class UpdateBookingStatusDto
    {
        public long BookingDetailId { get; set; }
        public string Note { get; set; }
        public BookingStatusEnum Status { get; set; }
    }
}