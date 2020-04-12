using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetBookingDetailForEditOutput
    {
		public CreateOrEditBookingDetailDto BookingDetail { get; set; }

		public string ProductName { get; set;}


    }
}