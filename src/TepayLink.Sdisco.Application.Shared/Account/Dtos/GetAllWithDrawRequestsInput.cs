using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllWithDrawRequestsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }

		public int StatusFilter { get; set; }

		public long? MaxTransactionIdFilter { get; set; }
		public long? MinTransactionIdFilter { get; set; }

		public long? MaxBankAccountIdFilter { get; set; }
		public long? MinBankAccountIdFilter { get; set; }



    }
}