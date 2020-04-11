using TepayLink.Sdisco.Help;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Help
{
	[Table("HelpContents")]
    public class HelpContent : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Question { get; set; }
		
		public virtual string Answer { get; set; }
		

		public virtual long? HelpCategoryId { get; set; }
		
        [ForeignKey("HelpCategoryId")]
		public HelpCategory HelpCategoryFk { get; set; }
		
    }
}