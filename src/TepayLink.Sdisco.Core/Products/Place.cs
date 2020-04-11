using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("Places")]
    public class Place : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(PlaceConsts.MaxNameLength, MinimumLength = PlaceConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual string DisplayAddress { get; set; }
		
		public virtual string GoogleAddress { get; set; }
		
		[Required]
		public virtual string Overview { get; set; }
		
		[Required]
		public virtual string WhatToExpect { get; set; }
		
		public virtual double Lat { get; set; }
		
		public virtual double Long { get; set; }
		

		public virtual long DetinationId { get; set; }
		
        [ForeignKey("DetinationId")]
		public Detination DetinationFk { get; set; }
		
		public virtual int? PlaceCategoryId { get; set; }
		
        [ForeignKey("PlaceCategoryId")]
		public PlaceCategory PlaceCategoryFk { get; set; }
		
    }
}