using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using Abp.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Products
{
	[Table("Products")]
    public class Product : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Name { get; set; }
		
		public virtual ProductTypeEnum Type { get; set; }
		
		public virtual ProductStatusEnum Status { get; set; }
		
		public virtual string Description { get; set; }
		public virtual string Overview { get; set; }
		public virtual string Policies { get; set; }
		
		public virtual int Duration { get; set; }
		
		public virtual string StartTime { get; set; }
		
		public virtual bool IncludeTourGuide { get; set; }
		
		public virtual bool AllowRetail { get; set; }
		
		public virtual int TotalSlot { get; set; }
		
		public virtual decimal Price { get; set; }
		public virtual decimal CostPrice { get; set; }
		public virtual bool InstantBook { get; set; }
		public virtual int Star { get; set; }

		public virtual int TripLengh { get; set; }
		
		public virtual bool IsHotDeal { get; set; }
		
		public virtual bool IsBestSeller { get; set; }
		
		public virtual bool IsTrending { get; set; }
		
		public virtual bool IsTop { get; set; }
		
		public virtual int BookingCount { get; set; }
		
		public virtual int CoppyCount { get; set; }
		
		public virtual int ShareCount { get; set; }
		
		public virtual int ViewCount { get; set; }
		public virtual int LikeCount { get; set; }
		public virtual long? ParentId { get; set; }
		
		public virtual long? TripCoppyId { get; set; }
		
		public virtual string FileName { get; set; }
		
		public virtual string ExtraData { get; set; }
		
		public virtual string WhatWeDo { get; set; }
		
		public virtual DateTime? LastBookTime { get; set; }
		

		public virtual int CategoryId { get; set; }
		
        [ForeignKey("CategoryId")]
		public Category CategoryFk { get; set; }
		
		public virtual long? HostUserId { get; set; }
		
        [ForeignKey("HostUserId")]
		public User HostUserFk { get; set; }
		
		public virtual long? PlaceId { get; set; }
		
        [ForeignKey("PlaceId")]
		public Place PlaceFk { get; set; }
		
		public virtual int? LanguageId { get; set; }
		
        [ForeignKey("LanguageId")]
		public ApplicationLanguage LanguageFk { get; set; }
		
    }
}