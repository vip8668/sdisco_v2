using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class BookingDto : EntityDto<long>
    {
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int TripLength { get; set; }

		public BookingStatusEnum Status { get; set; }

		public int Quantity { get; set; }

		public decimal Amount { get; set; }

		public decimal Fee { get; set; }

		public string Note { get; set; }

		public string GuestInfo { get; set; }

		public string CouponCode { get; set; }

		public decimal BonusAmount { get; set; }

		public string Contact { get; set; }

		public long CouponId { get; set; }

		public decimal TotalAmount { get; set; }


		 public long? ProductId { get; set; }

		 
    }
}