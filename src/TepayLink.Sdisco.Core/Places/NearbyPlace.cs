using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Places
{
	[Table("NearbyPlaces")]
    public class NearbyPlace : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Description { get; set; }
		

		public virtual long? PlaceId { get; set; }
		
        [ForeignKey("PlaceId")]
		public Place PlaceFk { get; set; }
		
		public virtual long? NearbyPlaceId { get; set; }
		
        [ForeignKey("NearbyPlaceId")]
		public Place NearbyPlaceFk { get; set; }
		
    }
}