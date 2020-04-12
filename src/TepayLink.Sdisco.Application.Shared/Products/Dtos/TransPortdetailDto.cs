
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class TransPortdetailDto : EntityDto<long>
    {
		public string From { get; set; }

		public string To { get; set; }

		public int TotalSeat { get; set; }

		public bool IsTaxi { get; set; }


		 public long? ProductId { get; set; }

		 
    }
}