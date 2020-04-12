
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string DisplayName { get; set; }
		
		
		public string Icon { get; set; }
		
		
		public bool IsDisabled { get; set; }
		
		

    }
}