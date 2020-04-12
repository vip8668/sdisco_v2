using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllProductDetailsForExcelInput
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public int? MaxOrderFilter { get; set; }
		public int? MinOrderFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}