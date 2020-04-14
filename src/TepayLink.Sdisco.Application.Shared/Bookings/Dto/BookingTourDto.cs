using System.Collections.Generic;


namespace TepayLink.Sdisco.Bookings.Dtos
{
  
    
    public class BookingTourDto
    {
        /// <summary>
        /// Tour ID
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// SỐ khách
        /// </summary>
        public int NumberOfGuest { get; set; }

        
        
        /// <summary>
        /// ID lịch trình
        /// </summary>
        public long TourScheduleId { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Khách
        /// </summary>
        public List<Guest> Guests { get; set; }


        /// <summary>
        /// Giá
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Phí
        /// </summary>
        public decimal Fee { get; set; }
        
    
    }

    
}