using System;
using System.Collections.Generic;
using TepayLink.Sdisco.Products;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{
    public class CreateTripPlanInputDto
    {
        //Ngày bắt đầu
        public DateTime? StartDate { get; set; }
        //Cho phép bán lẻ từng item của tour
        public bool AllowedForRetail { get; set; }
        
        //Chỉ thêm lịch không chỉnh sửa thông tin Tour
        public bool AddSchedule { get; set; }
        
        public int LanguageId { get; set; }
        
        public bool InstallBook { get; set; }
        
        /// <summary>
        /// số vé bán ra.
        /// </summary>
        public int TotalSlot { get; set; }
        
        public string Policy { get; set; }
        
        /// <summary>
        /// Id Triplan ( Dùng khi update Tạo mới để =0)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        ///  danh sách Plan chuyến đi ( ngày 1, ngày 2....)
        /// </summary>
        public List<DayOfTripPlanDto> Plans { get; set; }
        /// <summary>
        /// Tiêu đề chuyến đi
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Description { get; set; }
        
        public string Overview { get; set; }
        
        /// <summary>
        /// Ảnh Đại diện của chuyến đi ( Trong design đang thiếu cái này) 
        /// </summary> 
        public  List<string> Photos { get; set; }
        /// <summary>
        /// true : lưu nhấp, 
        /// false: public trip
        /// </summary>
        public  bool SaveDraft { get; set; }
        
    }
    
    public class DayOfTripPlanDto
    {
        /// <summary>
        /// Tiêu đề của ngày
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Ảnh Đại diện của ngày ( Trong design đang thiếu cái này) 
        /// </summary> 
        public List<string> Photos { get; set; }

        public  int Order { get; set; }
        /// <summary>
        /// Ngày đi
        /// </summary>
      
        public DateTime? Date { get; set; }
        /// <summary>
        /// danh sách Tour của ngày
        /// </summary>
        public  List<TripPlanItemDto> Tours { get; set; }
        /// <summary>
        /// Danh sách khách sạn của ngày
        /// </summary>
        public  List<HotelItemDTo> Hotels { get; set; }
        /// <summary>
        /// Danh sách Transport
        /// </summary>
        public  List<TransportItemDto> Transport { get; set; }
    }

    public class TripPlanItemDto
    {
        
        public long ItemId { get; set; }
        
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        /// <summary>
        ///Lấy giá trị trường ItemScheduleId ( AvaiableTimes.ItemScheduleId được chọn khi search Tour)
        /// </summary>
        public long TourDetailItemId { get; set; }
        /// <summary>
        /// Type (lấy giá trị Type của item được chọn)
        /// </summary>
        public ProductTypeEnum Type { get; set; }
        /// <summary>
        /// Note
        /// </summary>
        public string Description { get; set; }
        
    }
    
    public class HotelItemDTo
    {

        public long ItemId { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal CostPrice { get; set; }
        
        
        public long TourDetailItemId { get; set; }
        /// <summary>
        /// ID room được chọn
        /// </summary>
        public long RoomId { get; set; }
        /// <summary>
        /// Note
        /// </summary>
        public string Description { get; set; }

    }
    
    public class TransportItemDto
    {
        public long ItemId { get; set; }
        /// <summary>
        /// ItemScheduleId ( lấy giá trị trường ItemScheduleId của Transport được chọn khi search)
        /// </summary>
        public long TourDetailItemId { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal CostPrice { get; set; }
        
        /// <summary>
        /// Note
        /// </summary>
        public string Description { get; set; }


    }
    
}