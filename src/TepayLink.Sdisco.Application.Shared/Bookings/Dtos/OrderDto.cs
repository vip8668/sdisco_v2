using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class OrderDto : EntityDto<long>
    {
		public string OrderCode { get; set; }

		public OrderTypeEnum OrderType { get; set; }

		public decimal Amount { get; set; }

		public string Note { get; set; }

		public OrderStatus Status { get; set; }

		public string OrderRef { get; set; }

		public long UserId { get; set; }

		public string BankCode { get; set; }

		public long CardId { get; set; }

		public string CardNumber { get; set; }

		public string Currency { get; set; }

		public string IssueDate { get; set; }

		public string NameOnCard { get; set; }

		public string TransactionId { get; set; }

		public long BookingId { get; set; }



    }
}