using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("BookingRefunds")]
    public class BookingRefund : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long BookingDetailId { get; set; }
		
		public virtual int RefundMethodId { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual byte Status { get; set; }
		
		public virtual decimal Amount { get; set; }
		

    }
}