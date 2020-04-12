
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class CashoutMethodTypeDto : EntityDto
    {
		public string Title { get; set; }

		public string Note { get; set; }



    }
}