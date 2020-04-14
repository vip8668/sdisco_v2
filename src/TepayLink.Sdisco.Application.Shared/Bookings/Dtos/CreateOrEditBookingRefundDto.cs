
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditBookingRefundDto : EntityDto<long?>
    {

		public long BookingDetailId { get; set; }
		
		
		public int RefundMethodId { get; set; }
		
		
		public string Description { get; set; }
		
		
		public byte Status { get; set; }
		
		
		public decimal Amount { get; set; }
		
		

    }
}