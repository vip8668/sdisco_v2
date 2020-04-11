
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class BankBranchDto : EntityDto
    {
		public string BranchName { get; set; }

		public string Address { get; set; }

		public int Order { get; set; }


		 public int? BankId { get; set; }

		 
    }
}