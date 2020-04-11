using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class GetAllHelpCategoriesForExcelInput
    {
		public string Filter { get; set; }

		public string CategoryNameFilter { get; set; }

		public int TypeFilter { get; set; }



    }
}