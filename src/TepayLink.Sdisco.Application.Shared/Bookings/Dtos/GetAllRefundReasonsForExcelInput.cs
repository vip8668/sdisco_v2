using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllRefundReasonsForExcelInput
    {
		public string Filter { get; set; }

		public string ReasonTextFilter { get; set; }



    }
}