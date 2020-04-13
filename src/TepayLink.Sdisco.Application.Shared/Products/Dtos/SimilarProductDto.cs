
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class SimilarProductDto : EntityDto<long>
    {

		 public long? ProductId { get; set; }

		 		 public long? SimilarProductId { get; set; }

		 
    }
}