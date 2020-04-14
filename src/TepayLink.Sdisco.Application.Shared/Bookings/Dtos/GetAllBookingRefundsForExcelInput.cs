using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllBookingRefundsForExcelInput
    {
		public string Filter { get; set; }

		public long? MaxBookingDetailIdFilter { get; set; }
		public long? MinBookingDetailIdFilter { get; set; }

		public int? MaxRefundMethodIdFilter { get; set; }
		public int? MinRefundMethodIdFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public byte? MaxStatusFilter { get; set; }
		public byte? MinStatusFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }



    }
}