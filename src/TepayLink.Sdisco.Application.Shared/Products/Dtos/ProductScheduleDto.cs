
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class ProductScheduleDto : EntityDto<long>
    {
		public int TotalSlot { get; set; }

		public int TotalBook { get; set; }

		public int LockedSlot { get; set; }

		public int TripLength { get; set; }

		public string Note { get; set; }

		public decimal Price { get; set; }

		public decimal TicketPrice { get; set; }

		public decimal CostPrice { get; set; }

		public decimal HotelPrice { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string DepartureTime { get; set; }

		public decimal Revenue { get; set; }

		public bool AllowBook { get; set; }


		 public long ProductId { get; set; }

		 
    }
}