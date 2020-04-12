using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductDetailCombos
{
    public class CreateOrEditProductDetailComboModalViewModel
    {
       public CreateOrEditProductDetailComboDto ProductDetailCombo { get; set; }

	   		public string ProductName { get; set;}

		public string ProductDetailTitle { get; set;}

		public string ProductName2 { get; set;}


	   public bool IsEditMode => ProductDetailCombo.Id.HasValue;
    }
}