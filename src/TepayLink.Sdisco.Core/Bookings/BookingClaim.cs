
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("BookingClaims")]
    public class BookingClaim : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long BookingDetailId { get; set; }
		

		public virtual int? ClaimReasonId { get; set; }
		
        [ForeignKey("ClaimReasonId")]
		public ClaimReason ClaimReasonFk { get; set; }
		
    }
}