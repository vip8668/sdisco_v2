using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductReviewDetails
{
    public class CreateOrEditProductReviewDetailModalViewModel
    {
       public CreateOrEditProductReviewDetailDto ProductReviewDetail { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => ProductReviewDetail.Id.HasValue;
    }
}