
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class CreateOrEditPartnerShipDto : EntityDto<int?>
    {

		public string Logo { get; set; }
		
		
		public string Title { get; set; }
		
		
		public string Link { get; set; }
		
		
		public int Order { get; set; }
		
		

    }
}