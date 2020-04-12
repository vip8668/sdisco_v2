
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class BookingClaimDto : EntityDto<long>
    {
		public long BookingDetailId { get; set; }


		 public int? ClaimReasonId { get; set; }

		 
    }
}