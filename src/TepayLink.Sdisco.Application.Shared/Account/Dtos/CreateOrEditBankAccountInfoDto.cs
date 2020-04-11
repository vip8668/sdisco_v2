
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class CreateOrEditBankAccountInfoDto : EntityDto<long?>
    {

		public string AccountName { get; set; }
		
		
		public string AccountNo { get; set; }
		
		
		 public int? BankId { get; set; }
		 
		 		 public int? BankBranchId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}