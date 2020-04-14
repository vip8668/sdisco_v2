using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{

    public class CreateTourItemBasicDto
    { /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// giá
        /// </summary>
        public decimal Price { get; set; }
        
        public decimal CostPrice { get; set; }

        /// <summary>
        /// ảnh
        /// </summary>
        public List<string> Photos { get; set; }
        
        public List<string> ThumbImages { get; set; }

        public List<int> UtilitiesId { get; set; }
        public string Description { get; set; }
        
        public string StartTime { get; set; }
        public int Duration { get; set; }
        
        public bool InstantBook { get; set; }
        
        public  string Policy { get; set; }
        
        public int Language { get; set; }
        public bool IncludeTourGuide { get; set; }
        
        public string Address { get; set; }
    }
    public class CreateTransportDto:CreateTourItemBasicDto
    {
        public long TransportId { get; set; }

        /// <summary>
        /// Thời gian ( ngày đang chọn)
        /// </summary>
        public DateTime SelectedDate { get; set; }

       

        /// <summary>
        /// vị trí xuất phát
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// đích đến
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// số ghế ngồi
        /// </summary>
        public int TotalSeat { get; set; }
        public bool IsTaxi { get; set; }


    }
}