using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("RefundReasons")]
    public class RefundReason : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string ReasonText { get; set; }
		

    }
}