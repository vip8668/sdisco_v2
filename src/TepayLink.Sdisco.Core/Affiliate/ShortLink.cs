using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Affiliate
{
	[Table("ShortLinks")]
    public class ShortLink : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual string FullLink { get; set; }
		
		public virtual string ShortCode { get; set; }
		

    }
}