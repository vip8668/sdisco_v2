namespace TepayLink.Sdisco.Booking.Dtos
{ 
    public class ClaimInputDto
    {
        /// <summary>
        /// BookingId
        /// </summary>
        public long BookingId { get; set; }
        /// <summary>
        /// ID Claim
        /// </summary>
        public  int ClaimId { get; set; }
        /// <summary>
        /// Tour Id
        /// </summary>
        public long ItemId { get; set; }
    }
}