using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class ConfirmOrderDto : Entity<long>
    {
        public string OrderCode { get; set; }
        public OrderStatus Status { get; set; }
    }
}
