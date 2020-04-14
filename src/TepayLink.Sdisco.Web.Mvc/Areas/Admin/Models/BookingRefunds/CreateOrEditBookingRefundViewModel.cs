using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BookingRefunds
{
    public class CreateOrEditBookingRefundModalViewModel
    {
       public CreateOrEditBookingRefundDto BookingRefund { get; set; }

	   
	   public bool IsEditMode => BookingRefund.Id.HasValue;
    }
}