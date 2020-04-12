
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditClaimReasonDto : EntityDto<int?>
    {

		public string Title { get; set; }
		
		

    }
}