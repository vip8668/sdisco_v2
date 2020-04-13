using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.SuggestedProducts
{
    public class CreateOrEditSuggestedProductModalViewModel
    {
       public CreateOrEditSuggestedProductDto SuggestedProduct { get; set; }

	   		public string ProductName { get; set;}

		public string ProductName2 { get; set;}


	   public bool IsEditMode => SuggestedProduct.Id.HasValue;
    }
}