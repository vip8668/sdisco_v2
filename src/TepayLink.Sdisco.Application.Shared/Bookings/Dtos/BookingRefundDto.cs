
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class BookingRefundDto : EntityDto<long>
    {
		public long BookingDetailId { get; set; }

		public int RefundMethodId { get; set; }

		public string Description { get; set; }

		public byte Status { get; set; }

		public decimal Amount { get; set; }



    }
}