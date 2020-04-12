using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductUtilities
{
    public class CreateOrEditProductUtilityModalViewModel
    {
       public CreateOrEditProductUtilityDto ProductUtility { get; set; }

	   		public string ProductName { get; set;}

		public string UtilityName { get; set;}


	   public bool IsEditMode => ProductUtility.Id.HasValue;
    }
}