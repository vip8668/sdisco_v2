
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Reports.Dtos
{
    public class RevenueByMonthDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public decimal Revenue { get; set; }

		public DateTime Date { get; set; }



    }
}