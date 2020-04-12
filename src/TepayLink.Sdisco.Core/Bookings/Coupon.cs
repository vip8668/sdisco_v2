using TepayLink.Sdisco.Bookings;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("Coupons")]
    public class Coupon : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Code { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual CouponStatusEnum Status { get; set; }
		

    }
}