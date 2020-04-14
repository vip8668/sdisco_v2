using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.RefundReasons
{
    public class CreateOrEditRefundReasonModalViewModel
    {
       public CreateOrEditRefundReasonDto RefundReason { get; set; }

	   
	   public bool IsEditMode => RefundReason.Id.HasValue;
    }
}