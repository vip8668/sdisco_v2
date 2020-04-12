using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class GetAllUserDefaultCashoutMethodTypesForExcelInput
    {
		public string Filter { get; set; }


		 public string CashoutMethodTypeTitleFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}