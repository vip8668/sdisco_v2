using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetPartnerForEditOutput
    {
		public CreateOrEditPartnerDto Partner { get; set; }

		public string UserName { get; set;}

		public string DetinationName { get; set;}


    }
}