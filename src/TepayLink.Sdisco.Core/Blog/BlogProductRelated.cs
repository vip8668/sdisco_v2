using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Blog
{
	[Table("BlogProductRelateds")]
    public class BlogProductRelated : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			


		public virtual long? BlogPostId { get; set; }
		
        [ForeignKey("BlogPostId")]
		public BlogPost BlogPostFk { get; set; }
		
		public virtual long? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}