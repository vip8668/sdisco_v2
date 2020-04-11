using TepayLink.Sdisco.Blog;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Blog
{
	[Table("BlogComments")]
    public class BlogComment : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Email { get; set; }
		
		public virtual string FullName { get; set; }
		
		public virtual double Rating { get; set; }
		
		public virtual long? ReplyId { get; set; }
		
		public virtual string WebSite { get; set; }
		
		public virtual string Title { get; set; }
		
		public virtual string Comment { get; set; }
		

		public virtual long? BlogPostId { get; set; }
		
        [ForeignKey("BlogPostId")]
		public BlogPost BlogPostFk { get; set; }
		
    }
}