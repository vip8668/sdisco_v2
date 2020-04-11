using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class TourDetailDto
    {
        
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }
      

       /// <summary>
       /// Hoạt động
       /// </summary>
       public List<string> Activities { get; set; }
       /// <summary>
       /// Ngôn ngữ
       /// </summary>

       public string Language { get; set; }
       /// <summary>
       /// Over view
       /// </summary>
        public string Overview { get; set; }
       /// <summary>
       /// Host UserId
       /// </summary>
        
       public BasicHostUserInfo HostUserInfo { get; set; }
       /// <summary>
       /// Photo image
       /// </summary>
        public List<PhotoDto> Images { get; set; }
       /// <summary>
       /// Danh sách sao của KS trong tour
       /// </summary>
       public  List<int> HotelStars { get; set; }
       /// <summary>
       /// Số ngày đi
       /// </summary>
       public  int TripLength
       {
        get { if (TripPlan != null) return TripPlan.TripLength; return 0; } 
       }
       /// <summary>
       /// TripPlan
       /// </summary>
       public BasicTripPlanDto TripPlan { get; set; } 
       /// <summary>
       /// Review
       /// </summary>
      public ReviewSummaryDto Review { get; set; }
        
       public  string Policies { get; set; }
       
       public  bool IsLove { get; set; }
       public  bool InstallBook { get; set; }
       
        
    }
}
