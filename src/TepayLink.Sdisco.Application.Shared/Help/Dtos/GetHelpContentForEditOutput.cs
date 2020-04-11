using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class GetHelpContentForEditOutput
    {
		public CreateOrEditHelpContentDto HelpContent { get; set; }

		public string HelpCategoryCategoryName { get; set;}


    }
}