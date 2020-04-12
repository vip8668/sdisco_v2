using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class BookingDetailDto : EntityDto<long>
    {
		public long BookingId { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public DateTime TripLength { get; set; }

		public BookingStatusEnum Status { get; set; }

		public long ProductScheduleId { get; set; }

		public int Quantity { get; set; }

		public decimal Amount { get; set; }

		public decimal Fee { get; set; }

		public byte HostPaymentStatus { get; set; }

		public long HostUserId { get; set; }

		public long BookingUserId { get; set; }

		public bool IsDone { get; set; }

		public long AffiliateUserId { get; set; }

		public long RoomId { get; set; }

		public string Note { get; set; }

		public DateTime? CancelDate { get; set; }

		public decimal? RefundAmount { get; set; }


		 public long? ProductId { get; set; }

		 
    }
}