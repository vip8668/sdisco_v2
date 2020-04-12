using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllTransPortdetailsForExcelInput
    {
		public string Filter { get; set; }

		public string FromFilter { get; set; }

		public string ToFilter { get; set; }

		public int? MaxTotalSeatFilter { get; set; }
		public int? MinTotalSeatFilter { get; set; }

		public int IsTaxiFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}