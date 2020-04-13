using System;


namespace SDisco.Booking.Dtos
{
    public class GetAvaiableTimeOfTourDto
    {
        
        /// <summary>
        /// Id của Tour/Trip, activity,ticket...
        /// </summary>
        public long TourId { get; set; }
        /// <summary>
        /// Ngày chọn trên lịch
        /// </summary>
        public  DateTime SelectedDate { get; set; }

        /// <summary>
        /// 1: trip plan, tour
        /// 2: activiti,place....
        /// </summary>
        public int Type { get; set; }
        
       
    }

    public class GetTourDetailInputDto
    {

        /// <summary>
        /// Id của Tour/Trip, activity,ticket...
        /// </summary>
        public long ItemId { get; set; }
      
    }

    public class GetAvaiableTimeOfTourInput
    {
        
        /// <summary>
        /// Id của Tour/Trip, activity,ticket...
        /// </summary>
        public long TourId { get; set; }
        
        public  DateTime FromDate { get; set; }
        public  DateTime ToDate { get; set; }
        /// <summary>
        /// 1: trip plan, tour
        /// 2: activiti,place....
        /// </summary>
        public int Type { get; set; }


    }
}