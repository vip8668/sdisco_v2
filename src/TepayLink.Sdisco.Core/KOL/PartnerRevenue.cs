using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.KOL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.KOL
{
	[Table("PartnerRevenues")]
    public class PartnerRevenue : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long Userid { get; set; }
		
		public virtual RevenueTypeEnum RevenueType { get; set; }
		
		public virtual long ProductId { get; set; }
		
		public virtual decimal Point { get; set; }
		
		public virtual decimal Money { get; set; }
		
		public virtual RevenueStatusEnum Status { get; set; }
		

    }
}