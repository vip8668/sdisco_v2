
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Clients.Dtos
{
    public class ClientSettingDto : EntityDto<long>
    {
		public string Key { get; set; }

		public string Value { get; set; }



    }
}