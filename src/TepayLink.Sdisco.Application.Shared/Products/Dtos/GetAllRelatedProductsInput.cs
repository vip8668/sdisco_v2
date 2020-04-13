using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllRelatedProductsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string ProductNameFilter { get; set; }

		 		 public string ProductName2Filter { get; set; }

		 
    }
}