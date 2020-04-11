using TepayLink.Sdisco.AdminConfig;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.AdminConfig
{
	[Table("BankBranchs")]
    public class BankBranch : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string BranchName { get; set; }
		
		public virtual string Address { get; set; }
		
		public virtual int Order { get; set; }
		

		public virtual int? BankId { get; set; }
		
        [ForeignKey("BankId")]
		public Bank BankFk { get; set; }
		
    }
}