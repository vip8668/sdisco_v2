using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetBookingRefundForEditOutput
    {
		public CreateOrEditBookingRefundDto BookingRefund { get; set; }


    }
}