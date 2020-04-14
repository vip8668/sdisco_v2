using System;

using TepayLink.Sdisco.Dto;

namespace SDisco.Affiliate.Dto
{
    public class PointListDetail
    {
        public DateTime Date { get; set; }
        public string TourTitle { get; set; }
        public int Count { get; set; }
        public int Point { get; set; }
        public string Slug { get; set; }

        
        public byte Type { get; set; }
    }

    public class GetPointListDetail : PagedInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Loại
        /// Shared=1,
        ///Coppy=2,
        /// Booking=3,
        /// Click=4
        /// </summary>
        public int Type { get; set; }
    }
}