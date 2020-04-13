using TepayLink.Sdisco.KOL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.KOL
{
	[Table("ShareTransactions")]
    public class ShareTransaction : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual RevenueTypeEnum Type { get; set; }
		
		public virtual string IP { get; set; }
		
		public virtual decimal Point { get; set; }
		
		public virtual long ProductId { get; set; }
		

    }
}