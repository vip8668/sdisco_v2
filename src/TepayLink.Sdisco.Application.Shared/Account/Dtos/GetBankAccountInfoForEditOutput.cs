using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetBankAccountInfoForEditOutput
    {
		public CreateOrEditBankAccountInfoDto BankAccountInfo { get; set; }

		public string BankBankName { get; set;}

		public string BankBranchBranchName { get; set;}

		public string UserName { get; set;}


    }
}