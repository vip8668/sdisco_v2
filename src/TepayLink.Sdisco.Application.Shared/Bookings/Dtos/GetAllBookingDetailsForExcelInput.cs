using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllBookingDetailsForExcelInput
    {
		public string Filter { get; set; }

		public decimal? MaxRefundAmountFilter { get; set; }
		public decimal? MinRefundAmountFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}