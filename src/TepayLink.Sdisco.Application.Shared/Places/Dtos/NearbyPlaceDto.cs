
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Places.Dtos
{
    public class NearbyPlaceDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long? PlaceId { get; set; }

		 		 public long? NearbyPlaceId { get; set; }

		 
    }
}