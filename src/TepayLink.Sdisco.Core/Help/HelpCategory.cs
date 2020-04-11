using TepayLink.Sdisco.Help;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Help
{
	[Table("HelpCategories")]
    public class HelpCategory : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string CategoryName { get; set; }
		
		public virtual HelpTypeEnum Type { get; set; }
		

    }
}