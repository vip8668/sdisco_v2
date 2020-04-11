using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("PlaceCategories")]
    public class PlaceCategory : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Name { get; set; }
		
		public virtual string Image { get; set; }
		
		public virtual string Icon { get; set; }
		

    }
}