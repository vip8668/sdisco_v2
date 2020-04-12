using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductReviews
{
    public class CreateOrEditProductReviewModalViewModel
    {
       public CreateOrEditProductReviewDto ProductReview { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => ProductReview.Id.HasValue;
    }
}