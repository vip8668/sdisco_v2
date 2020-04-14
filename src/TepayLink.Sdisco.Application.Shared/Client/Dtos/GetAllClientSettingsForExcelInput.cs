using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Clients.Dtos
{
    public class GetAllClientSettingsForExcelInput
    {
		public string Filter { get; set; }

		public string KeyFilter { get; set; }

		public string ValueFilter { get; set; }



    }
}