using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Bookings
{
	[Table("Orders")]
    public class Order : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string OrderCode { get; set; }
		
		public virtual OrderTypeEnum OrderType { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual string Note { get; set; }
		
		public virtual OrderStatus Status { get; set; }
		
		public virtual string OrderRef { get; set; }
		
		public virtual long UserId { get; set; }
		
		public virtual string BankCode { get; set; }
		
		public virtual long CardId { get; set; }
		
		public virtual long CardNumber { get; set; }
		
		public virtual string Currency { get; set; }
		
		public virtual string IssueDate { get; set; }
		
		public virtual string NameOnCard { get; set; }
		
		public virtual string TransactionId { get; set; }
		
		public virtual long BookingId { get; set; }
		

    }
}