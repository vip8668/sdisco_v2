
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class CreateOrEditUserDefaultCashoutMethodTypeDto : EntityDto<long?>
    {

		 public int? CashoutMethodTypeId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}