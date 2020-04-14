using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Clients.Dtos
{
    public class GetClientSettingForEditOutput
    {
		public CreateOrEditClientSettingDto ClientSetting { get; set; }


    }
}