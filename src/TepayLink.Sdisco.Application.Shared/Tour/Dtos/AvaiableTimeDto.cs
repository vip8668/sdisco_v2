using System;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class AvaiableTimeDto
    {
        public  long ItemId { get; set; }
        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public  DateTime ToDate { get; set; }
        
        public string DepartureTime { get; set; }
        /// <summary>
        /// Giờ khởi hành bắt đầu
        /// </summary>
        
       public DateTime DepartureTimeStart { get{
           if (string.IsNullOrEmpty(DepartureTime)) return FromDate;

           try
           {
               var arr = DepartureTime.Split('-');
               var arr1 = arr[0].Split(':');
               return FromDate.AddHours(int.Parse(arr1[0])).AddMinutes(int.Parse(arr1[1]));
           }
           catch (Exception e)
           {
               return FromDate.AddHours(9);
           }
       } }
        /// <summary>
        /// Giờ khởi hành kết thúc
        /// </summary>
       public DateTime DepartureTimeEnd { get{
           if (string.IsNullOrEmpty(DepartureTime)) return FromDate;

           try
           {
               var arr = DepartureTime.Split('-');
               var arr1 = arr[1].Split(':');
               return FromDate.AddHours(int.Parse(arr1[0])).AddMinutes(int.Parse(arr1[1]));
           }
           catch (Exception e)
           {
               return FromDate.AddHours(10).AddMinutes(30);
           }
           

       } }

        /// <summary>
        /// Id lịch trình
        /// </summary>
        public long ScheduleId { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        public BasicPriceDto Price { get; set; }
        /// <summary>
        /// Số slot chuyến đi
        /// </summary>
        public int TotalSlot { get; set; }
        /// <summary>
        /// tổng đã book
        /// </summary>
        public  int TotalBook { get; set; }
        public  int AvaiableSlot { get; set; }
    }
}