using TepayLink.Sdisco.AdminConfig;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class BankDto : EntityDto
    {
		public string BankName { get; set; }

		public string BankCode { get; set; }

		public string DisplayName { get; set; }

		public BankTypeEnum Type { get; set; }

		public int Order { get; set; }

		public string Logo { get; set; }

		public string CardImage { get; set; }

		public string Description { get; set; }



    }
}