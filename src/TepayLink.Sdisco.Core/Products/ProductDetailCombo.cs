using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("ProductDetailCombos")]
    public class ProductDetailCombo : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long? RoomId { get; set; }
		
		public virtual string Description { get; set; }
		

		public virtual long? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
		public virtual long? ProductDetailId { get; set; }
		
        [ForeignKey("ProductDetailId")]
		public ProductDetail ProductDetailFk { get; set; }
		
		public virtual long? ItemId { get; set; }
		
        [ForeignKey("ItemId")]
		public Product ItemFk { get; set; }
		
    }
}