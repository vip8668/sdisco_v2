using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Reports
{
	[Table("RevenueByMonths")]
    public class RevenueByMonth : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual decimal Revenue { get; set; }
		
		public virtual DateTime Date { get; set; }
		

    }
}