using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductDto : EntityDto<long>
    {
		public string Name { get; set; }

		public ProductTypeEnum Type { get; set; }

		public ProductStatusEnum Status { get; set; }

		public string Description { get; set; }

		public string Policies { get; set; }

		public int Duration { get; set; }

		public string StartTime { get; set; }

		public bool IncludeTourGuide { get; set; }

		public bool AllowRetail { get; set; }

		public int TotalSlot { get; set; }

		public decimal Price { get; set; }

		public bool InstantBook { get; set; }

		public int TripLengh { get; set; }

		public bool IsHotDeal { get; set; }

		public bool IsBestSeller { get; set; }

		public bool IsTrending { get; set; }

		public bool IsTop { get; set; }


		 public int? CategoryId { get; set; }

		 		 public long? HostUserId { get; set; }

		 		 public long? PlaceId { get; set; }

		 		 public int? LanguageId { get; set; }

		 
    }
}