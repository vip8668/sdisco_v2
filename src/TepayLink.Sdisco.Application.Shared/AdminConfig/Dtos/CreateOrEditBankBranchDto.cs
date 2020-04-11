
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class CreateOrEditBankBranchDto : EntityDto<int?>
    {

		public string BranchName { get; set; }
		
		
		public string Address { get; set; }
		
		
		public int Order { get; set; }
		
		
		 public int? BankId { get; set; }
		 
		 
    }
}