using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Client.Dtos
{
    public class GetAllClientSettingsForExcelInput
    {
		public string Filter { get; set; }

		public string KeyFilter { get; set; }

		public string ValueFilter { get; set; }



    }
}