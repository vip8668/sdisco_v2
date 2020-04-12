
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class CreateOrEditCashoutMethodTypeDto : EntityDto<int?>
    {

		public string Title { get; set; }
		
		
		public string Note { get; set; }
		
		

    }
}