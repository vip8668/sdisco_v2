using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Bookings
{
    public enum BookingStatusEnum
    {
        WaitPayment = 1,
        WaitConfirm = 2,
        Accepted = 3,
        Cancel = 4,
        Refunded = 5,
        Complete = 6,
        Hide = 7,
        Refused = 8,
        Deleted = 9,
    }
}
