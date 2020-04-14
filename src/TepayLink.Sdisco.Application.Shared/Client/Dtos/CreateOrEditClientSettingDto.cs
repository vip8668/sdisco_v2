
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Clients.Dtos
{
    public class CreateOrEditClientSettingDto : EntityDto<long?>
    {

		[Required]
		public string Key { get; set; }
		
		
		public string Value { get; set; }
		
		

    }
}