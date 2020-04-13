using TepayLink.Sdisco.Account;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class CreateOrEditWithDrawRequestDto : EntityDto<long?>
    {

		public long UserId { get; set; }
		
		
		public decimal Amount { get; set; }
		
		
		public WithDrawRequestStatus Status { get; set; }
		
		
		public long TransactionId { get; set; }
		
		
		public long BankAccountId { get; set; }
		
		

    }
}