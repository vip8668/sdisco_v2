using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Places.Dtos
{
    public class GetNearbyPlaceForEditOutput
    {
		public CreateOrEditNearbyPlaceDto NearbyPlace { get; set; }

		public string PlaceName { get; set;}

		public string PlaceName2 { get; set;}


    }
}