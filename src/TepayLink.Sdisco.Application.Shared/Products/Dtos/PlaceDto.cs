
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class PlaceDto : EntityDto<long>
    {
		public string Name { get; set; }

		public string DisplayAddress { get; set; }

		public string GoogleAddress { get; set; }

		public string Overview { get; set; }

		public string WhatToExpect { get; set; }


		 public long DetinationId { get; set; }

		 		 public int? PlaceCategoryId { get; set; }

		 
    }
}