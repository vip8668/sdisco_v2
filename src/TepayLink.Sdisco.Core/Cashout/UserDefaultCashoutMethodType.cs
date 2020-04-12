using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Cashout
{
	[Table("UserDefaultCashoutMethodTypes")]
    public class UserDefaultCashoutMethodType : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			


		public virtual int? CashoutMethodTypeId { get; set; }
		
        [ForeignKey("CashoutMethodTypeId")]
		public CashoutMethodType CashoutMethodTypeFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}