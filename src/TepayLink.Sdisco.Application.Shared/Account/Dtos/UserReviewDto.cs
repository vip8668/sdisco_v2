
using System;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class UserReviewDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public double ReviewCount { get; set; }

		public double Itineraty { get; set; }

		public double Service { get; set; }

		public double Transport { get; set; }

		public double GuideTour { get; set; }

		public double Food { get; set; }

		public double Rating { get; set; }



    }

    public class GetUserReviewDetailInput : PagedInputDto
    {

        public long? UserId { get; set; }
    }

    public class GetTripCreated : PagedInputDto
    {

        public long? UserId { get; set; }
    }

    public class ConnectFbDto
    {
        /// <summary>
        /// Provider ( Facebook/ Google)
        /// </summary>

        public string AuthProvider { get; set; }

        /// <summary>
        /// mã người dùng ( Id người dùng trả về khi login bằng google)
        /// </summary>

        public string ProviderKey { get; set; }
        /// <summary>
        /// Mã truy cập
        /// </summary>


        public string ProviderAccessCode { get; set; }

    }
    public class CashoutTypeInputDto
    {
        /// <summary>
        /// Id phương thức thanh toán
        /// </summary>
        public int CashoutTypeId { get; set; }
    }
    public class CashoutTypeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public bool IsChecked { get; set; }
    }
    public class SetRoleDto
    {
        /// <summary>
        /// User Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Role Name
        /// </summary>
        public string RoleName { get; set; }
    }
}