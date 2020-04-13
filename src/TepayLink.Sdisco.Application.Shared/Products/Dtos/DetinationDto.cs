using TepayLink.Sdisco.Products;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class DetinationDto : EntityDto<long>
    {
		public string Image { get; set; }

		public string Name { get; set; }

		public DetinationStatusEnum Status { get; set; }

		public bool IsTop { get; set; }

		public int BookingCount { get; set; }



    }
}