using TepayLink.Sdisco.AdminConfig;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.AdminConfig
{
	[Table("Banks")]
    public class Bank : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string BankName { get; set; }
		
		public virtual string BankCode { get; set; }
		
		public virtual string DisplayName { get; set; }
		
		public virtual BankTypeEnum Type { get; set; }
		
		public virtual int Order { get; set; }
		
		public virtual string Logo { get; set; }
		
		public virtual string CardImage { get; set; }
		
		public virtual string Description { get; set; }
		

    }
}