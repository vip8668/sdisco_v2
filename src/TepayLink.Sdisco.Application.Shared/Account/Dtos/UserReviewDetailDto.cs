
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class UserReviewDetailDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public double Itineraty { get; set; }

		public double Service { get; set; }

		public double Transport { get; set; }

		public double GuideTour { get; set; }

		public double Food { get; set; }

		public double Rating { get; set; }

		public string Title { get; set; }

		public string Comment { get; set; }
        /// <summary>
        /// Avata người đánh giá
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Họ tên người đánh giá
        /// </summary>
        public string Reviewer { get; set; }
        /// <summary>
        /// Ngày đánh giá
        /// </summary>
        public DateTime ReviewDate { get; set; }


    }
}