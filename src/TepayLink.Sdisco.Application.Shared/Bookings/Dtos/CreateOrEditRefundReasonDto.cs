
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditRefundReasonDto : EntityDto<int?>
    {

		public string ReasonText { get; set; }
		
		

    }
}