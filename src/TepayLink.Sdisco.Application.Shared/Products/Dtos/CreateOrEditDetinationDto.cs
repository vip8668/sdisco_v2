using TepayLink.Sdisco.Products;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditDetinationDto : EntityDto<long?>
    {

		[Required]
		public string Image { get; set; }
		
		
		[StringLength(DetinationConsts.MaxNameLength, MinimumLength = DetinationConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public DetinationStatusEnum Status { get; set; }
		
		
		public bool IsTop { get; set; }
		
		
		public int BookingCount { get; set; }
		
		

    }
}