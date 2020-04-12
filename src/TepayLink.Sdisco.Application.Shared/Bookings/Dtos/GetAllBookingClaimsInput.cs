using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllBookingClaimsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxBookingDetailIdFilter { get; set; }
		public long? MinBookingDetailIdFilter { get; set; }


		 public string ClaimReasonTitleFilter { get; set; }

		 
    }
}