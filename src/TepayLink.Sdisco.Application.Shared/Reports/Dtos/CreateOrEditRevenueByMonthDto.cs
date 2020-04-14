
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Reports.Dtos
{
    public class CreateOrEditRevenueByMonthDto : EntityDto<long?>
    {

		public long UserId { get; set; }
		
		
		public decimal Revenue { get; set; }
		
		
		public DateTime Date { get; set; }
		
		

    }
}