using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("Detinations")]
    public class Detination : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string Image { get; set; }
		
		[StringLength(DetinationConsts.MaxNameLength, MinimumLength = DetinationConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual DetinationStatusEnum Status { get; set; }
		
		public virtual bool IsTop { get; set; }
		
		public virtual int BookingCount { get; set; }
		

    }
}