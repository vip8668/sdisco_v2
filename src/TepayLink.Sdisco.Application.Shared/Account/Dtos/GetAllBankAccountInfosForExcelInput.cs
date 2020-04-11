using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllBankAccountInfosForExcelInput
    {
		public string Filter { get; set; }

		public string AccountNameFilter { get; set; }

		public string AccountNoFilter { get; set; }


		 public string BankBankNameFilter { get; set; }

		 		 public string BankBranchBranchNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}