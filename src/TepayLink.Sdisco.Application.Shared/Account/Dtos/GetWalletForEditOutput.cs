using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetWalletForEditOutput
    {
		public CreateOrEditWalletDto Wallet { get; set; }

		public string UserName { get; set;}


    }
}