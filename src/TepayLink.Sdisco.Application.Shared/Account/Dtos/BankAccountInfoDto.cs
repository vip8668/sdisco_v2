
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class BankAccountInfoDto : EntityDto<long>
    {
		public string AccountName { get; set; }

		public string AccountNo { get; set; }


		 public int? BankId { get; set; }

		 		 public int? BankBranchId { get; set; }

		 		 public long? UserId { get; set; }

		 
    }
}