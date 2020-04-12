
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class PartnerShipDto : EntityDto
    {
		public string Logo { get; set; }

		public string Title { get; set; }

		public string Link { get; set; }

		public int Order { get; set; }



    }
}