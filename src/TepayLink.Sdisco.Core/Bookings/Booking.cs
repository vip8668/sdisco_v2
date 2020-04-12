using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("Bookings")]
    public class Booking : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual DateTime StartDate { get; set; }
		
		public virtual DateTime EndDate { get; set; }
		
		public virtual int TripLength { get; set; }
		
		public virtual BookingStatusEnum Status { get; set; }
		
		public virtual long ProductScheduleId { get; set; }
		
		public virtual int Quantity { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual decimal Fee { get; set; }
		
		public virtual string Note { get; set; }
		
		public virtual string GuestInfo { get; set; }
		
		public virtual string CouponCode { get; set; }
		
		public virtual decimal BonusAmount { get; set; }
		
		public virtual string Contact { get; set; }
		
		public virtual long CouponId { get; set; }
		
		public virtual decimal TotalAmount { get; set; }
		

		public virtual long? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}