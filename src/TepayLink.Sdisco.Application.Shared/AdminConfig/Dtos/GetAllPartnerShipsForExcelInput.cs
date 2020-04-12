using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class GetAllPartnerShipsForExcelInput
    {
		public string Filter { get; set; }

		public string LogoFilter { get; set; }

		public string TitleFilter { get; set; }

		public string LinkFilter { get; set; }



    }
}