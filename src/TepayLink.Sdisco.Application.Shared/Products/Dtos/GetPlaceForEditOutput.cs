using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetPlaceForEditOutput
    {
		public CreateOrEditPlaceDto Place { get; set; }

		public string DetinationName { get; set;}

		public string PlaceCategoryName { get; set;}


    }
}