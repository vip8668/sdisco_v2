
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class CreateOrEditHelpContentDto : EntityDto<long?>
    {

		public string Question { get; set; }
		
		
		public string Answer { get; set; }
		
		
		 public long? HelpCategoryId { get; set; }
		 
		 
    }
}