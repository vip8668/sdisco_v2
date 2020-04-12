using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class GetUserDefaultCashoutMethodTypeForEditOutput
    {
		public CreateOrEditUserDefaultCashoutMethodTypeDto UserDefaultCashoutMethodType { get; set; }

		public string CashoutMethodTypeTitle { get; set;}

		public string UserName { get; set;}


    }
}