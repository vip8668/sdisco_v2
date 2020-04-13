using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.SimilarProducts
{
    public class CreateOrEditSimilarProductModalViewModel
    {
       public CreateOrEditSimilarProductDto SimilarProduct { get; set; }

	   		public string ProductName { get; set;}

		public string ProductName2 { get; set;}


	   public bool IsEditMode => SimilarProduct.Id.HasValue;
    }
}