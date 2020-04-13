using System;
using System.Collections.Generic;

namespace TepayLink.Sdisco.Booking.Dtos
{
    public class WriteReviewInputDto
    {
        public long BookingId { get; set; }
        public long ItemId { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public int Itineraty { get; set; }
        public int Service { get; set; }
        public int Transport { get; set; }
        public int GuidTour { get; set; }
        public int Food { get; set; }

        public List<string> Photos { get; set; }

        public double AvgRatting()
        {
            return Math.Round( ((double) (Itineraty + Service + GuidTour + Food + Transport) / 5),2);
        }
    }
}