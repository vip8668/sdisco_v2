
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductUtilityDto : EntityDto<long>
    {

		 public long? ProductId { get; set; }

		 		 public int? UtilityId { get; set; }

		 
    }
}