
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditUtilityDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Icon { get; set; }
		
		

    }
}