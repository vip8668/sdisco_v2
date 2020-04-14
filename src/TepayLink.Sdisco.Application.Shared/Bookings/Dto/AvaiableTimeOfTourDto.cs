using System;
using System.Collections.Generic;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class AvaiableTimeOfTourDto
    {
        /// <summary>
        /// Thông tin về tour ( Tên, ảnh đại diện)
        /// </summary>
        public BasicItemDto Item { get; set; }
       
        
        /// <summary>
        /// Ngày bắt đầu đi
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime ToTime { get; set; }

        /// <summary>
        /// Giá
        /// </summary>
        public BasicPriceDto Price { get; set; }

        /// <summary>
        /// Item Type : tour, tripplan/ activity, ticket
        /// </summary>
        //  public ProductTypeEnum ItemType { get; set; }
        public int ItemType
        { get; set; }
        /// <summary>
        /// Số slot trống
        /// </summary>
        public int TotalSlot { get; set; }

        /// <summary>
        /// Số slot trống
        /// </summary>
        public int TotalBook { get; set; }

        /// <summary>
        /// Số slot trống
        /// </summary>
        public int TotalAvaiable
        {
            get { return TotalSlot - TotalBook; }
        }

        /// <summary>
        /// ID lịch trình
        /// </summary>
        public long TourScheduleId { get; set; }

        public long TourId { get; set; }
        /// <summary>
        /// Có thể thanh toàn ngay ( true có thể thanh toán ngay, false phải chờ duyệt)
        /// </summary>
        public bool InstallBook { get; set; }
    }

    public class AvaiableTourDto
    {
        /// <summary>
        /// Host User
        /// </summary>
        public List< BasicHostUserInfo> HostUsers { get; set; }
        /// <summary>
        /// Danh sách time hiện tại của ngày chọn
        /// </summary>
        public List<AvaiableTimeOfTourDto> Current { get; set; }

        /// <summary>
        /// Danh sách time lịch tiếp theo trong tháng
        /// </summary>
        public List<AvaiableTimeOfTourDto> NextAvaiabled { get; set; }
        
    }
}