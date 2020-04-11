using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllDetinationsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int StatusFilter { get; set; }



    }
}