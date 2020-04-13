using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllProductReviewDetailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string ProductNameFilter { get; set; }

		 
    }
}