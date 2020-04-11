using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class GetBankBranchForEditOutput
    {
		public CreateOrEditBankBranchDto BankBranch { get; set; }

		public string BankBankName { get; set;}


    }
}