using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllOrdersForExcelInput
    {
		public string Filter { get; set; }

		public string IssueDateFilter { get; set; }

		public string NameOnCardFilter { get; set; }

		public string TransactionIdFilter { get; set; }



    }
}