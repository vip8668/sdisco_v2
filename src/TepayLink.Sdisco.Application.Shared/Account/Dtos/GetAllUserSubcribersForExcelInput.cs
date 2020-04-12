using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllUserSubcribersForExcelInput
    {
		public string Filter { get; set; }

		public string EmailFilter { get; set; }



    }
}