using TepayLink.Sdisco.Account;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class WithDrawRequestDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public decimal Amount { get; set; }

		public WithDrawRequestStatus Status { get; set; }

		public long TransactionId { get; set; }

		public long BankAccountId { get; set; }



    }
}