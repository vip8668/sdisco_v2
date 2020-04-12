using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetBookingForEditOutput
    {
		public CreateOrEditBookingDto Booking { get; set; }

		public string ProductName { get; set;}


    }
}