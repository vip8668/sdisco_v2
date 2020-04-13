
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class SuggestedProductDto : EntityDto<long>
    {

		 public long ProductId { get; set; }

		 		 public long? SuggestedProductId { get; set; }

		 
    }
}