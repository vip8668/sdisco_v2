using TepayLink.Sdisco.Help;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class HelpCategoryDto : EntityDto<long>
    {
		public string CategoryName { get; set; }

		public HelpTypeEnum Type { get; set; }



    }
}