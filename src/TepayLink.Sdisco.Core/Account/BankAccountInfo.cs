using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Account
{
	[Table("BankAccountInfos")]
    public class BankAccountInfo : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string AccountName { get; set; }
		
		public virtual string AccountNo { get; set; }
		

		public virtual int? BankId { get; set; }
		
        [ForeignKey("BankId")]
		public Bank BankFk { get; set; }
		
		public virtual int? BankBranchId { get; set; }
		
        [ForeignKey("BankBranchId")]
		public BankBranch BankBranchFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}