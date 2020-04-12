using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.UserReviews
{
    public class CreateOrEditUserReviewModalViewModel
    {
       public CreateOrEditUserReviewDto UserReview { get; set; }

	   
	   public bool IsEditMode => UserReview.Id.HasValue;
    }
}