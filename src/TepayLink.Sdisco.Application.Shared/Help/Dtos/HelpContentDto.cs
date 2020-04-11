
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class HelpContentDto : EntityDto<long>
    {
		public string Question { get; set; }

		public string Answer { get; set; }


		 public long? HelpCategoryId { get; set; }

		 
    }
}