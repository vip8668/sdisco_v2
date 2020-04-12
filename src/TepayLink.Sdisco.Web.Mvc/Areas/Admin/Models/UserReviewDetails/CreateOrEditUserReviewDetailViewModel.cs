using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.UserReviewDetails
{
    public class CreateOrEditUserReviewDetailModalViewModel
    {
       public CreateOrEditUserReviewDetailDto UserReviewDetail { get; set; }

	   
	   public bool IsEditMode => UserReviewDetail.Id.HasValue;
    }
}