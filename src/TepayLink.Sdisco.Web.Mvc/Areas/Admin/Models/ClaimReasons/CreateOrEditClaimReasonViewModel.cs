using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ClaimReasons
{
    public class CreateOrEditClaimReasonModalViewModel
    {
       public CreateOrEditClaimReasonDto ClaimReason { get; set; }

	   
	   public bool IsEditMode => ClaimReason.Id.HasValue;
    }
}