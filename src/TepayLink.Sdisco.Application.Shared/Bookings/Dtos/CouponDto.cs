using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CouponDto : EntityDto<long>
    {
		public string Code { get; set; }

		public decimal Amount { get; set; }

		public CouponStatusEnum Status { get; set; }



    }
}