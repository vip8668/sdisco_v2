using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("BookingDetails")]
    public class BookingDetail : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long BookingId { get; set; }
		
		public virtual DateTime StartDate { get; set; }
		
		public virtual DateTime EndDate { get; set; }
		
		public virtual int TripLength { get; set; }
		
		public virtual BookingStatusEnum Status { get; set; }
		
		public virtual long ProductScheduleId { get; set; }
		
		public virtual int Quantity { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual decimal Fee { get; set; }
		
		public virtual byte HostPaymentStatus { get; set; }
		
		public virtual long HostUserId { get; set; }
		
		public virtual long BookingUserId { get; set; }
		
		public virtual bool IsDone { get; set; }
		
		public virtual long AffiliateUserId { get; set; }
		
		public virtual long RoomId { get; set; }
		
		public virtual string Note { get; set; }
		
		public virtual DateTime? CancelDate { get; set; }
		
		public virtual decimal? RefundAmount { get; set; }
		
		public virtual long? ProductDetailComboId { get; set; }
		

		public virtual long? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}