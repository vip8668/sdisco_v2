
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditPlaceCategoryDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Image { get; set; }
		
		
		public string Icon { get; set; }
		
		

    }
}