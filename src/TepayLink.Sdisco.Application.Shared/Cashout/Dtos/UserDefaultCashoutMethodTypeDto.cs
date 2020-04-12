
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class UserDefaultCashoutMethodTypeDto : EntityDto<long>
    {

		 public int? CashoutMethodTypeId { get; set; }

		 		 public long? UserId { get; set; }

		 
    }
}