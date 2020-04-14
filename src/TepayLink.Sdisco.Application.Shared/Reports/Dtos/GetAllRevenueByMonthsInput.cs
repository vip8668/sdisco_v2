using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Reports.Dtos
{
    public class GetAllRevenueByMonthsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public decimal? MaxRevenueFilter { get; set; }
		public decimal? MinRevenueFilter { get; set; }

		public DateTime? MaxDateFilter { get; set; }
		public DateTime? MinDateFilter { get; set; }



    }
}