using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllPlacesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }


		 public string DetinationNameFilter { get; set; }

		 		 public string PlaceCategoryNameFilter { get; set; }

		 
    }
}