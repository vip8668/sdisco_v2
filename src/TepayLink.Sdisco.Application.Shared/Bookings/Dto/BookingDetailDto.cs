
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.Utils;

namespace TepayLink.Sdisco.Booking.Dto
{
   public class BookingDetailDto
    {
        public TourDetailDto TourDetail { get; set; }

        public ProductTypeEnum Type { get; set; }
        public string Slug => TourDetail.Title.GenerateSlug();

        public long TourScheduleId { get; set; }
        public int NumberOfGuest { get; set; }

        public decimal Amount { get; set; }
        public decimal Fee { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public BookingStatusEnum Status { get; set; }

        

    }
}
