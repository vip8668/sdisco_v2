using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Places
{
    public class CreateOrEditPlaceModalViewModel
    {
       public CreateOrEditPlaceDto Place { get; set; }

	   		public string DetinationName { get; set;}

		public string PlaceCategoryName { get; set;}


	   public bool IsEditMode => Place.Id.HasValue;
    }
}