using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllWalletsForExcelInput
    {
		public string Filter { get; set; }

		public decimal? MaxBalanceFilter { get; set; }
		public decimal? MinBalanceFilter { get; set; }

		public int TypeFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}