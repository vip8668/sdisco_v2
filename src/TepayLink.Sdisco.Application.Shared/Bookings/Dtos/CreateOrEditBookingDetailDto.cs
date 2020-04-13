using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditBookingDetailDto : EntityDto<long?>
    {

		public decimal? RefundAmount { get; set; }
		
		
		public long ProductDetailComboId { get; set; }
		
		
		 public long? ProductId { get; set; }
		 
		 
    }
}