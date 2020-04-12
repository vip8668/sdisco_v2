using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditCouponDto : EntityDto<long?>
    {

		public string Code { get; set; }
		
		
		public decimal Amount { get; set; }
		
		
		public CouponStatusEnum Status { get; set; }
		
		

    }
}