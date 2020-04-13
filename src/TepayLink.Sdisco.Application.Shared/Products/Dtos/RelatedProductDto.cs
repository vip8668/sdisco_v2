
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class RelatedProductDto : EntityDto<long>
    {

		 public long? ProductId { get; set; }

		 		 public long? RelatedProductId { get; set; }

		 
    }
}