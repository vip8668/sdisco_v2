using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{
  public  class SearchTransportInputDto
    {
        /// <summary>
        /// Ngày đang chọn
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Điểm đầu
        /// </summary>
       
        public string From { get; set; }
        /// <summary>
        /// điểm đến
        /// </summary>
        public string To { get; set; }
        
    }
}
