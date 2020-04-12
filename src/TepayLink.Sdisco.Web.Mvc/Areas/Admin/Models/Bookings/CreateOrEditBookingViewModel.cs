using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Bookings
{
    public class CreateOrEditBookingModalViewModel
    {
       public CreateOrEditBookingDto Booking { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => Booking.Id.HasValue;
    }
}