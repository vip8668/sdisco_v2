
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductDetailDto : EntityDto<long>
    {
		public string Title { get; set; }

		public int Order { get; set; }

		public string Description { get; set; }

		public string ThumbImage { get; set; }


		 public long? ProductId { get; set; }

		 
    }
}