using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllUtilitiesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string IconFilter { get; set; }



    }
}