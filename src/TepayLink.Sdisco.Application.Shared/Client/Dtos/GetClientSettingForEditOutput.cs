using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Client.Dtos
{
    public class GetClientSettingForEditOutput
    {
		public CreateOrEditClientSettingDto ClientSetting { get; set; }


    }
}