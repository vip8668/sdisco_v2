using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Authorization.Users.Profile.Dto
{
    public class GetProfileViewDto
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Họ tên
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Đanh giá
        /// </summary>
        public float Rating { get; set; }
        /// <summary>
        /// xếp hạng
        /// </summary>
        public string Ranking { get; set; }
        /// <summary>
        /// Về bản thân
        /// </summary>
        public string AboutMe { get; set; }
        /// <summary>
        /// Nơi sống
        /// </summary>
        public string LiveIn { get; set; }
        /// <summary>
        /// Loại tài khoản
        /// 1: Host
        /// 2: Traveler
        /// </summary>

        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string Languages { get; set; }
        /// <summary>
        /// Công việc
        /// </summary>
        public string Work { get; set; }
        /// <summary>
        /// Tình trạng xác thực email
        /// </summary>
        public bool VerifyEmail { get; set; }
        /// <summary>
        /// Tình trạng xác thực ID
        /// </summary>
        public bool VerifyGovermentId { get; set; }
        /// <summary>
        /// Tình trạng xác thực số điện thoại
        /// </summary>
        public bool VerifyPhone { get; set; }
        public bool VerifySocialMedia { get; set; }

        public string Avatar { get; set; }
        public int Point { get; set; }
        public Review Review { get; set; }
    }

    public class Review
    {
        public UserReviewDetailDto TopReview { get; set; }

        public UserReviewDto Reviews { get; set; }
    }
}
