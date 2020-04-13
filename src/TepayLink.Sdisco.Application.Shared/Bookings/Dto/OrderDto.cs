using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Bookings.Dto
{
    public class OrderDto : Entity<long>
    {
        public string OrderCode { get; set; }
        public decimal Amount { get; set; }
        public OrderTypeEnum OrderType { get; set; }
        public OrderStatus Status { get; set; }
    }
}
