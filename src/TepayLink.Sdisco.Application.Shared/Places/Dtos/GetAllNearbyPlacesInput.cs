using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Places.Dtos
{
    public class GetAllNearbyPlacesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }


		 public string PlaceNameFilter { get; set; }

		 		 public string PlaceName2Filter { get; set; }

		 
    }
}