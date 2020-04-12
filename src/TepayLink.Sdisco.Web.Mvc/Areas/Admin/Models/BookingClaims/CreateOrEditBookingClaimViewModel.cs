using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BookingClaims
{
    public class CreateOrEditBookingClaimModalViewModel
    {
       public CreateOrEditBookingClaimDto BookingClaim { get; set; }

	   		public string ClaimReasonTitle { get; set;}


	   public bool IsEditMode => BookingClaim.Id.HasValue;
    }
}