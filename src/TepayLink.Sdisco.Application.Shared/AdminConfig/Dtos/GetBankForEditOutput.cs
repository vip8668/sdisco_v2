using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class GetBankForEditOutput
    {
		public CreateOrEditBankDto Bank { get; set; }


    }
}