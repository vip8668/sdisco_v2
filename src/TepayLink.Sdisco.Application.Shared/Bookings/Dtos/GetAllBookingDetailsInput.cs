using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllBookingDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public decimal? MaxRefundAmountFilter { get; set; }
		public decimal? MinRefundAmountFilter { get; set; }

		public long? MaxProductDetailComboIdFilter { get; set; }
		public long? MinProductDetailComboIdFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}