using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.RelatedProducts
{
    public class CreateOrEditRelatedProductModalViewModel
    {
       public CreateOrEditRelatedProductDto RelatedProduct { get; set; }

	   		public string ProductName { get; set;}

		public string ProductName2 { get; set;}


	   public bool IsEditMode => RelatedProduct.Id.HasValue;
    }
}