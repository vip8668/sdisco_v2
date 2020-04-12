using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Products
{
    public class CreateOrEditProductModalViewModel
    {
       public CreateOrEditProductDto Product { get; set; }

	   		public string CategoryName { get; set;}

		public string UserName { get; set;}

		public string PlaceName { get; set;}

		public string ApplicationLanguageName { get; set;}


	   public bool IsEditMode => Product.Id.HasValue;
    }
}