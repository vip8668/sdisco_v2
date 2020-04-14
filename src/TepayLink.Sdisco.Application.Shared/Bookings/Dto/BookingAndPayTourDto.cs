using System;
using System.Collections.Generic;
using TepayLink.Sdisco.Payment.Dto;
using TepayLink.Sdisco.Products;

namespace TepayLink.Sdisco.Bookings.Dtos
{

    public class PayPendingBooking
    {
        public long BookingId { get; set; }
        public string CouponCode { get; set; }
        public PaymentInputDto Payment { get; set; }
    }
    public class BookingAndPayTourDto
    {
        public DateTime? BookingDate { get; set; }
        /// <summary>
        /// Item Id
        /// </summary>
        public long ItemId { get; set; }
        
       
        public ProductTypeEnum ItemType { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int NumberOfGuest { get; set; }

        /// <summary>
        /// Mã giảm giá
        /// </summary>
        public string CouponCode { get; set; }

        public bool InstallBook { get; set; }
        
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

         public Contact Contact { get; set; }


        public PaymentInputDto Payment { get; set; }
    }

    public class Guest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
    }
    //public class OrderInfo
    //{
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //}

    public class Contact
    {
        public string Address { get; set; }
        public string Mobile { get; set; }

    }
  
}