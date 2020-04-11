using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class GetAllBanksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string BankNameFilter { get; set; }

		public string BankCodeFilter { get; set; }

		public string DisplayNameFilter { get; set; }

		public int TypeFilter { get; set; }



    }
}