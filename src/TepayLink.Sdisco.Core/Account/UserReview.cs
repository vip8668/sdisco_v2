using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Account
{
	[Table("UserReviews")]
    public class UserReview : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual double ReviewCount { get; set; }
		
		public virtual double Itineraty { get; set; }
		
		public virtual double Service { get; set; }
		
		public virtual double Transport { get; set; }
		
		public virtual double GuideTour { get; set; }
		
		public virtual double Food { get; set; }
		
		public virtual double Rating { get; set; }
		

    }
}