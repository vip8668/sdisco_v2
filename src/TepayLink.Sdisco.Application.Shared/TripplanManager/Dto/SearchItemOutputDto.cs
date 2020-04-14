using System;
using System.Collections.Generic;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{
    /// <summary>
    /// Search ItemOuput
    /// </summary>
    public class SearchItemOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        public long TourDetailItemId { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Vi trí
        /// </summary>
        public BasicLocationDto Location { get; set; }

        public ProductTypeEnum Type { get; set; }

        /// <summary>
        /// Loại ( tour, Hotel, transport...)
        /// </summary>
        public string TypeText => Type.ToString("G");
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public List<PhotoDto> ThumbImages { get; set; }
        /// <summary>
        /// Note
        /// </summary>

        public string Description { get; set; }
        /// <summary>
        /// Đánh giá
        /// </summary>

        public ReviewSummaryDto Review { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string Language { get; set; }
        public int LanguageId { get; set; }

        public string StartTime { get; set; }
        public  long Duration { get; set; }
        
        /// <summary>
        /// Có bao gồm tourGuide không
        /// </summary>
        public bool IncludeTourGuide { get; set; }
        /// <summary>
        /// Thời gian Avaiable
        /// </summary>
        //public List<AvaiableTime> AvaiableTimes { get; set; }
        
        public BasicPriceDto Price { get; set; }
        /// <summary>
        /// số slot trống
        /// </summary>
        public int Avaiable { get; set; }
        
        public int TotalSlot { get; set; }
        
        public List<UtilityDto> Utilities { get; set; }
        
        public bool InstantBook { get; set; }
        //  public List<RoomDto> Rooms { get; set; }
    }
    /// <summary>
    /// AvaiableTime
    /// </summary>
    public class AvaiableTime
    {
        /// <summary>
        /// 
        /// </summary>
        public long ItemScheduleId { get; set; }
        /// <summary>
        /// Từ thời gian
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Tới thời gian
        /// </summary>
        public DateTime ToDate { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        public BasicPriceDto Price { get; set; }
        /// <summary>
        /// số slot trống
        /// </summary>
        public int Avaiable { get; set; }
        
        public int TotalSlot { get; set; }
        
        /// <summary>
        /// Khung thời gian đang được chọn cho chuyến đi
        /// </summary>

        public bool IsSelected { get; set; }
    }



    /// <summary>
    /// Phòng KS
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tên phòng
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        public BasicPriceDto Price { get; set; }
        /// <summary>
        /// Được chọn cho chuyến đi
        /// </summary>
        public bool IsSelected { get; set; }

    }

    /// <summary>
    /// Output Hotel
    /// </summary>
    public class SearchHotelOutputDto
    {
        /// <summary>
        /// Schedule Id
        /// </summary>
        public long TourDetailItemId { get; set; }
        /// <summary>
        /// Tên KS
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ID KS
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Vị trí
        /// </summary>
        public BasicLocationDto Location { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>

        public List<PhotoDto> ThumbImages { get; set; }
        /// <summary>
        /// Note
        /// </summary>

        public string Description { get; set; }
        /// <summary>
        /// Đánh giá
        /// </summary>

        public ReviewSummaryDto Review { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>

        public string Language { get; set; }
        public int LanguageId { get; set; }
        public bool InstantBook { get; set; }
        /// <summary>
        /// Danh sách phòng
        /// </summary>


        public List<RoomDto> Rooms { get; set; }

        /// <summary>
        /// Trạng thái
        ///  Draft=0,
        /// Active=1,
        ///  WaitApprove=2,
        ///  Delete=3
        /// </summary>
        public ProductStatusEnum Status { get; set; }
        
        public List<UtilityDto> Utilities { get; set; }
        public int Star { get; set; }
        
        public  string Address { get; set; }
        
    }
    /// <summary>
    ///  Transport ouput
    /// </summary>
    public class SearchTransportOutputDto
    {
        /// <summary>
        /// ItemScheduleId
        /// </summary>
        public long TourDetailItemId { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        public bool IsTaxi { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public List<PhotoDto> ThumbImages { get; set; }
        /// <summary>
        /// Note
        /// </summary>

        public string Description { get; set; }
        /// <summary>
        /// Dánh giá
        /// </summary>

        public ReviewSummaryDto Review { get; set; }
        /// <summary>
        /// Avaiable Time
        /// </summary>

      //  public List<AvaiableTime> AvaiableTimes { get; set; }
        
        public string From { get; set; }
        public string To { get; set; }
        
        public int TotalSeat { get; set; }
        
        public bool InstantBook { get; set; }

        /// <summary>
        /// Trạng thái
        ///  Draft=0,
        /// Active=1,
        ///  WaitApprove=2,
        ///  Delete=3
        /// </summary>
        public ProductStatusEnum Status { get; set; }
        // public List<int> AvaiableSeats { get; set; }
        public List<UtilityDto> Utilities { get; set; }
        
        public string StartTime { get; set; }
        public  long Duration { get; set; }
        
        public BasicPriceDto Price { get; set; }
        /// <summary>
        /// số slot trống
        /// </summary>
        public int Avaiable { get; set; }
        
        public int TotalSlot { get; set; }
        public string Address { get; set; }

        public string Language { get; set; }
        public int LanguageId { get; set; }

    }

}