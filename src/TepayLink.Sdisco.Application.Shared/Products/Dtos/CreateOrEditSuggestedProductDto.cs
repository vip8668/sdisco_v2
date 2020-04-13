
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditSuggestedProductDto : EntityDto<long?>
    {

		 public long ProductId { get; set; }
		 
		 		 public long? SuggestedProductId { get; set; }
		 
		 
    }
}