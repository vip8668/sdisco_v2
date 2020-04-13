using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetTransactionForEditOutput
    {
		public CreateOrEditTransactionDto Transaction { get; set; }


    }
}