
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class CountryDto : EntityDto
    {
		public string Name { get; set; }

		public string DisplayName { get; set; }

		public string Icon { get; set; }

		public bool IsDisabled { get; set; }



    }
}