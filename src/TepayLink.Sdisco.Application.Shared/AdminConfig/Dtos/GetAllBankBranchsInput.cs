using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class GetAllBankBranchsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string BankBankNameFilter { get; set; }

		 
    }
}