using TepayLink.Sdisco.Blog;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Blog
{
	[Table("BlogPosts")]
    public class BlogPost : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Title { get; set; }
		
		public virtual string ShortDescription { get; set; }
		
		public virtual string Content { get; set; }
		
		public virtual DateTime PublishDate { get; set; }
		
		public virtual string ThumbImage { get; set; }
		
		public virtual BlogStatusEnum Status { get; set; }
		

    }
}