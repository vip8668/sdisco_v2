using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("ProductReviews")]
    public class ProductReview : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual double RatingAvg { get; set; }
		
		public virtual int ReviewCount { get; set; }
		
		public virtual double Intineraty { get; set; }
		
		public virtual double Service { get; set; }
		
		public virtual double Transport { get; set; }
		
		public virtual double GuideTour { get; set; }
		
		public virtual double Food { get; set; }
		

		public virtual long ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}