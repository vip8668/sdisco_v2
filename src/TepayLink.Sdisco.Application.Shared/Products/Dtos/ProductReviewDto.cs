
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductReviewDto : EntityDto<long>
    {
		public double RatingAvg { get; set; }

		public int ReviewCount { get; set; }

		public double Intineraty { get; set; }

		public double Service { get; set; }

		public double Transport { get; set; }

		public double GuideTour { get; set; }

		public double Food { get; set; }


		 public long? ProductId { get; set; }

		 
    }
}