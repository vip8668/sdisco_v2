using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Products
{
    public enum ProductTypeEnum
    {
        Tour = 1,
        Trip = 2,
        Activity = 3,
        TicketPlace = 4,
        TicketShow = 5,
        ThingToBuy = 6,
        Hotel = 7,
        Transport = 8,

    }

    public enum ProductStatusEnum
    {
        Draft = 0,
        Publish = 1,
        WaitApprove = 2,
        Delete = 3
    }

    public enum ImageType
    {
        ThumbImage = 1,
        TourImage = 2,
        GuestImage = 3
    }
}
