using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrderInputDto
    {
        public OrderTypeEnum OrderType { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public long UserId { get; set; }
        public string Currency { get; set; }
        public long BookingId { get; set; }
    }
}
