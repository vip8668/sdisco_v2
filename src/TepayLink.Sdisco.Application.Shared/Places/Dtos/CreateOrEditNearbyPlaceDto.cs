
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Places.Dtos
{
    public class CreateOrEditNearbyPlaceDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long? PlaceId { get; set; }
		 
		 		 public long? NearbyPlaceId { get; set; }
		 
		 
    }
}