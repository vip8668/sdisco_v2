
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductReviewDetailDto : EntityDto<long>
    {
		public double RatingAvg { get; set; }

		public double Intineraty { get; set; }

		public double Service { get; set; }

		public double Transport { get; set; }

		public decimal GuideTour { get; set; }

		public double Food { get; set; }

		public string Title { get; set; }

		public string Comment { get; set; }

		public long BookingId { get; set; }

		public bool Read { get; set; }

		public string ReplyComment { get; set; }

		public long? ReplyId { get; set; }

		public string Avatar { get; set; }

		public string Reviewer { get; set; }



    }
}