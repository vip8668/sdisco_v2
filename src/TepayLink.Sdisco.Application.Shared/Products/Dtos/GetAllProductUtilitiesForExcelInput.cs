using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllProductUtilitiesForExcelInput
    {
		public string Filter { get; set; }


		 public string ProductNameFilter { get; set; }

		 		 public string UtilityNameFilter { get; set; }

		 
    }
}