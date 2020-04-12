using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("ProductSchedules")]
    public class ProductSchedule : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual int TotalSlot { get; set; }
		
		public virtual int TotalBook { get; set; }
		
		public virtual int LockedSlot { get; set; }
		
		public virtual int TripLength { get; set; }
		
		public virtual string Note { get; set; }
		
		public virtual decimal Price { get; set; }
		
		public virtual decimal TicketPrice { get; set; }
		
		public virtual decimal CostPrice { get; set; }
		
		public virtual decimal HotelPrice { get; set; }
		
		public virtual DateTime StartDat { get; set; }
		
		public virtual DateTime EndDate { get; set; }
		
		public virtual string DepartureTime { get; set; }
		
		public virtual decimal Revenue { get; set; }
		

		public virtual long? ProductId { get; set; }
		
        [ForeignKey("ProductId")]
		public Product ProductFk { get; set; }
		
    }
}