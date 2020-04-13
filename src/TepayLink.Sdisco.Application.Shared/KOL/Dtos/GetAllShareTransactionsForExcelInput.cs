using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class GetAllShareTransactionsForExcelInput
    {
		public string Filter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public int TypeFilter { get; set; }

		public string IPFilter { get; set; }

		public decimal? MaxPointFilter { get; set; }
		public decimal? MinPointFilter { get; set; }

		public long? MaxProductIdFilter { get; set; }
		public long? MinProductIdFilter { get; set; }



    }
}