using TepayLink.Sdisco.Places.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.NearbyPlaces
{
    public class CreateOrEditNearbyPlaceModalViewModel
    {
       public CreateOrEditNearbyPlaceDto NearbyPlace { get; set; }

	   		public string PlaceName { get; set;}

		public string PlaceName2 { get; set;}


	   public bool IsEditMode => NearbyPlace.Id.HasValue;
    }
}