using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class GetCashoutMethodTypeForEditOutput
    {
		public CreateOrEditCashoutMethodTypeDto CashoutMethodType { get; set; }


    }
}