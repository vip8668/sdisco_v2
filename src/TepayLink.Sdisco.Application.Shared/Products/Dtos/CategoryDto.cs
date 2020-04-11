using TepayLink.Sdisco.Products;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CategoryDto : EntityDto
    {
		public string Name { get; set; }

		public string Image { get; set; }

		public string Icon { get; set; }

		public ProductTypeEnum ProductType { get; set; }



    }
}