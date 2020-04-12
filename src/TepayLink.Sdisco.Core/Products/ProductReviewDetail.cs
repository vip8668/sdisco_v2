using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("ProductReviewDetails")]
    public class ProductReviewDetail : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual double RatingAvg { get; set; }
		
		public virtual double Intineraty { get; set; }
		
		public virtual double Service { get; set; }
		
		public virtual double Transport { get; set; }
		
		public virtual decimal GuideTour { get; set; }
		
		public virtual double Food { get; set; }
		
		public virtual string Title { get; set; }
		
		public virtual string Comment { get; set; }
		
		public virtual long BookingId { get; set; }
		
		public virtual bool Read { get; set; }
		
		public virtual string ReplyComment { get; set; }
		
		public virtual long? ReplyId { get; set; }
		
		public virtual string Avatar { get; set; }
		
		public virtual string Reviewer { get; set; }
		

    }
}