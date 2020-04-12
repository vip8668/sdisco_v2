
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditProductUtilityDto : EntityDto<long?>
    {

		 public long? ProductId { get; set; }
		 
		 		 public int? UtilityId { get; set; }
		 
		 
    }
}