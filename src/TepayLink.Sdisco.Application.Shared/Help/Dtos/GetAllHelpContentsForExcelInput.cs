using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class GetAllHelpContentsForExcelInput
    {
		public string Filter { get; set; }


		 public string HelpCategoryCategoryNameFilter { get; set; }

		 
    }
}