using System.Collections.Generic;
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Affiliate.Dto
{
    public class CommissionDto
    {
        /// <summary>
        /// Tour/trip Id
        /// </summary>
        public long ItemId { get; set; }
       
        public ProductTypeEnum ItemType { get; set; }
        /// <summary>
        /// Tên tour
        /// </summary>
        public string TourTitle { get; set; }
        /// <summary>
        /// Số điểm
        /// </summary>
        public decimal Point { get; set; }
        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// Loại hoa hồng
        /// 1 :Share Trip,
        /// 2 :Coppy trip
        /// 3 :Booking
        /// 4 :Click
        /// </summary>
        public RevenueTypeEnum RevenueType { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public List<PhotoDto> ThumbImages { get; set; }
    }
}