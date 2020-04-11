using TepayLink.Sdisco.Help;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class CreateOrEditHelpCategoryDto : EntityDto<long?>
    {

		public string CategoryName { get; set; }
		
		
		public HelpTypeEnum Type { get; set; }
		
		

    }
}