
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditProductScheduleDto : EntityDto<long?>
    {

		public bool AllowBook { get; set; }
		
		
		 public long? ProductId { get; set; }
		 
		 
    }
}