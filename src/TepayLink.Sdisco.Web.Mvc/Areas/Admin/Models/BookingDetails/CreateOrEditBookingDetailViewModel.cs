using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BookingDetails
{
    public class CreateOrEditBookingDetailModalViewModel
    {
       public CreateOrEditBookingDetailDto BookingDetail { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => BookingDetail.Id.HasValue;
    }
}