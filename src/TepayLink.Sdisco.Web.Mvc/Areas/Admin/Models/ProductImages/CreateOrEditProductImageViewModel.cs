using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductImages
{
    public class CreateOrEditProductImageModalViewModel
    {
       public CreateOrEditProductImageDto ProductImage { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => ProductImage.Id.HasValue;
    }
}