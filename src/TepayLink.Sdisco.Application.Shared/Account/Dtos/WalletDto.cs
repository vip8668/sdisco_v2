using TepayLink.Sdisco.Account;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class WalletDto : EntityDto<long>
    {
		public decimal Balance { get; set; }

		public WalletTypeEnum Type { get; set; }


		 public long? UserId { get; set; }

		 
    }
}