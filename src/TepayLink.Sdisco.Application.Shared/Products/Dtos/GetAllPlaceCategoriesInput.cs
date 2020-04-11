using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllPlaceCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}