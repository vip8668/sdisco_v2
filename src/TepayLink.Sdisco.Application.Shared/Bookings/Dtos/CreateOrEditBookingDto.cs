using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditBookingDto : EntityDto<long?>
    {

		public BookingStatusEnum Status { get; set; }
		
		
		 public long? ProductId { get; set; }
		 
		 
    }
}