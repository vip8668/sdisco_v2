using System;
using System.Collections.Generic;

using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{
    public class CustomizeTripOutputDto
    {

        public DateTime? StartDate { get; set; }
        
        /// <summary>
        ///Chi tiết từng ngày đi
        /// </summary>
        public List<DayOfTripPlanCustomizeDto> Plans { get; set; }
        /// <summary>
        /// Tiêu đề chuyến đi
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public List<PhotoDto> Photos { get; set; }
        public int TotalSlot { get; set; }
        /// <summary>
        /// Host User
        /// </summary>
        public BasicHostUserInfo HostUser { get; set; }

        public bool InstallBook { get; set; }
        public string Policy { get; set; }
        public string Overview { get; set; }
        public int LanguageId { get; set; }
       
    }

    public class DayOfTripPlanCustomizeDto
    {
        /// <summary>
        /// Thumb Images
        /// </summary>
        public List<PhotoDto> Photos { get; set; }
        /// <summary>
        /// Thứ tự từng ngày
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }
       /// <summary>
       /// Ngày đi
       /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Danh sách Tour  
        /// </summary>
        public List<SearchItemOutputDto> Tours { get; set; }
        /// <summary>
        /// Danh sách Hotel
        /// </summary>
        public List<SearchHotelOutputDto> Hotels { get; set; }
        //Danh sách Transport
        public List<SearchTransportOutputDto> Transport { get; set; }
    }



}