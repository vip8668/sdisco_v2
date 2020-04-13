using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllTransactionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }

		public byte? MaxSideFilter { get; set; }
		public byte? MinSideFilter { get; set; }

		public int TransTypeFilter { get; set; }

		public int WalletTypeFilter { get; set; }

		public long? MaxBookingDetailIdFilter { get; set; }
		public long? MinBookingDetailIdFilter { get; set; }

		public long? MaxRefIdFilter { get; set; }
		public long? MinRefIdFilter { get; set; }

		public string DescritionFilter { get; set; }



    }
}