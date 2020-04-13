using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllProductSchedulesForExcelInput
    {
		public string Filter { get; set; }

		public int AllowBookFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}