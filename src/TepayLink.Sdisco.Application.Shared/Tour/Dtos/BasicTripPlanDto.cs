using System.Collections.Generic;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class BasicTripPlanDto
    {
        
        public long TourId { get; set; }
        public int HotelStar { get; set; }
       // public ShareType ShareType { get; set; }
        public int TripLength { get; set; }
        public BasicPriceDto Price { get; set; }
       
        public List<TripPlanDetailDto> TripPlanDetailDtos { get; set; }
    }
}