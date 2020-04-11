using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("ProductImages")]
    public class ProductImage : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Url { get; set; }
		
		public virtual ImageType ImageType { get; set; }
		
		public virtual string Tag { get; set; }
		
		public virtual string Title { get; set; }
		

		public virtual long? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}