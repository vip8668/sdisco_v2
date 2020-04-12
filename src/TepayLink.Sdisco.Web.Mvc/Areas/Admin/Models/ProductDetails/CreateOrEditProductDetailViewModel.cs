using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductDetails
{
    public class CreateOrEditProductDetailModalViewModel
    {
       public CreateOrEditProductDetailDto ProductDetail { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => ProductDetail.Id.HasValue;
    }
}