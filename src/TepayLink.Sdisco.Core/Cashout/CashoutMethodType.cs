using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Cashout
{
	[Table("CashoutMethodTypes")]
    public class CashoutMethodType : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Title { get; set; }
		
		public virtual string Note { get; set; }
		

    }
}