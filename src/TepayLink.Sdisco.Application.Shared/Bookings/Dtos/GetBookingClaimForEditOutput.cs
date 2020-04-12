using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetBookingClaimForEditOutput
    {
		public CreateOrEditBookingClaimDto BookingClaim { get; set; }

		public string ClaimReasonTitle { get; set;}


    }
}