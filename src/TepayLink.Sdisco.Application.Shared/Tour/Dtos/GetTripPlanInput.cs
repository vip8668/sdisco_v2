namespace TepayLink.Sdisco.Tour.Dtos
{
    public class GetTripPlanInput
    {
        /// <summary>
        /// TourId
        /// </summary>
        public long TourId { get; set; }
        /// <summary>
        /// Số sao KS
        /// </summary>
        public  int HotelStar { get; set; }

        public byte SharedType { get; set; }

    }
}