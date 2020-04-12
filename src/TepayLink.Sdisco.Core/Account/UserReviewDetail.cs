using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Account
{
	[Table("UserReviewDetails")]
    public class UserReviewDetail : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual double Itineraty { get; set; }
		
		public virtual double Service { get; set; }
		
		public virtual double Transport { get; set; }
		
		public virtual double GuideTour { get; set; }
		
		public virtual double Food { get; set; }
		
		public virtual double Rating { get; set; }
		
		public virtual string Title { get; set; }
		
		public virtual string Comment { get; set; }
		

    }
}