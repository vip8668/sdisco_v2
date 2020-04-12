using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Client
{
	[Table("ClientSettings")]
    public class ClientSetting : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string Key { get; set; }
		
		public virtual string Value { get; set; }
		

    }
}