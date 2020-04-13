using TepayLink.Sdisco.Account;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Account
{
	[Table("WithDrawRequests")]
    public class WithDrawRequest : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual WithDrawRequestStatus Status { get; set; }
		
		public virtual long TransactionId { get; set; }
		
		public virtual long BankAccountId { get; set; }
		

    }
}