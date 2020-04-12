using TepayLink.Sdisco.Account;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class CreateOrEditWalletDto : EntityDto<long?>
    {

		public decimal Balance { get; set; }
		
		
		public WalletTypeEnum Type { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}