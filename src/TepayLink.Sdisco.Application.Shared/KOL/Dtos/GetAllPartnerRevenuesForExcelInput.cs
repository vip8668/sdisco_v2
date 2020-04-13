using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class GetAllPartnerRevenuesForExcelInput
    {
		public string Filter { get; set; }

		public long? MaxUseridFilter { get; set; }
		public long? MinUseridFilter { get; set; }

		public int RevenueTypeFilter { get; set; }

		public decimal? MaxPointFilter { get; set; }
		public decimal? MinPointFilter { get; set; }

		public decimal? MaxMoneyFilter { get; set; }
		public decimal? MinMoneyFilter { get; set; }

		public int StatusFilter { get; set; }



    }
}