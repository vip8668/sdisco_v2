using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetWithDrawRequestForEditOutput
    {
		public CreateOrEditWithDrawRequestDto WithDrawRequest { get; set; }


    }
}