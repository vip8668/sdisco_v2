using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Coupons
{
    public class CreateOrEditCouponModalViewModel
    {
       public CreateOrEditCouponDto Coupon { get; set; }

	   
	   public bool IsEditMode => Coupon.Id.HasValue;
    }
}