using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class GetAllHelpContentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string HelpCategoryCategoryNameFilter { get; set; }

		 
    }
}