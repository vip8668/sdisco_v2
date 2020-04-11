using TepayLink.Sdisco.Products;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Image { get; set; }
		
		
		public string Icon { get; set; }
		
		
		public ProductTypeEnum ProductType { get; set; }
		
		

    }
}