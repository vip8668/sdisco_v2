using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllProductsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int TypeFilter { get; set; }

		public int StatusFilter { get; set; }

		public int IncludeTourGuideFilter { get; set; }

		public int AllowRetailFilter { get; set; }

		public decimal? MaxPriceFilter { get; set; }
		public decimal? MinPriceFilter { get; set; }

		public int IsTrendingFilter { get; set; }

		public string ExtraDataFilter { get; set; }

		public string WhatWeDoFilter { get; set; }

		public DateTime? MaxLastBookTimeFilter { get; set; }
		public DateTime? MinLastBookTimeFilter { get; set; }


		 public string CategoryNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 		 public string PlaceNameFilter { get; set; }

		 		 public string ApplicationLanguageNameFilter { get; set; }

		 
    }
}