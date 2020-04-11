using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllProductImagesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int ImageTypeFilter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}