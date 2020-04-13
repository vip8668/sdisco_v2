using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Bookings
{
    public enum OrderTypeEnum
    {
        AddCard = 1,
        Booking = 2
    }
    public enum OrderStatus
    {
        Init = 0,
        Success = 1,
        Fail = 2,
        Cancel = 3,
        Pending = 4,
        Unknow = 5,
    }
    public enum CouponStatusEnum
    {
        Init=0,
        Used=1
    }
}
