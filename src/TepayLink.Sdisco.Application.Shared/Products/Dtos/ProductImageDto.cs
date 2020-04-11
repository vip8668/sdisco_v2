using TepayLink.Sdisco.Products;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductImageDto : EntityDto<long>
    {
		public string Url { get; set; }

		public ImageType ImageType { get; set; }

		public string Tag { get; set; }

		public string Title { get; set; }


		 public long? ProductId { get; set; }

		 
    }
}