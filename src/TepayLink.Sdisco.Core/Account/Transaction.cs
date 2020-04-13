using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Account
{
	[Table("Transactions")]
    public class Transaction : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual byte Side { get; set; }
		
		public virtual TransactionType TransType { get; set; }
		
		public virtual WalletTypeEnum WalletType { get; set; }
		
		public virtual long? BookingDetailId { get; set; }
		
		public virtual long? RefId { get; set; }
		
		public virtual string Descrition { get; set; }
		

    }
}