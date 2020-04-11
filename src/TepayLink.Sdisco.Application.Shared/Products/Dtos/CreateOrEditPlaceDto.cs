
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditPlaceDto : EntityDto<long?>
    {

		[StringLength(PlaceConsts.MaxNameLength, MinimumLength = PlaceConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public string DisplayAddress { get; set; }
		
		
		public string GoogleAddress { get; set; }
		
		
		[Required]
		public string Overview { get; set; }
		
		
		[Required]
		public string WhatToExpect { get; set; }
		
		
		public double Lat { get; set; }
		
		
		public double Long { get; set; }
		
		
		 public long DetinationId { get; set; }
		 
		 		 public int? PlaceCategoryId { get; set; }
		 
		 
    }
}