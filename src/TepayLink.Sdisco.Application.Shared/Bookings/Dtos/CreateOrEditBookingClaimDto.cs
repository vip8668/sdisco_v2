
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditBookingClaimDto : EntityDto<long?>
    {

		public long BookingDetailId { get; set; }
		
		
		 public int? ClaimReasonId { get; set; }
		 
		 
    }
}