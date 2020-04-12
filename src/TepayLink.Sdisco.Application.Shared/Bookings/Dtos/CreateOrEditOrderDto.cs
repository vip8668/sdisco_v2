using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class CreateOrEditOrderDto : EntityDto<long?>
    {

		public string OrderCode { get; set; }
		
		
		public OrderTypeEnum OrderType { get; set; }
		
		
		public string Note { get; set; }
		
		
		public OrderStatus Status { get; set; }
		
		
		public string OrderRef { get; set; }
		
		
		public string IssueDate { get; set; }
		
		
		public string NameOnCard { get; set; }
		
		
		public string TransactionId { get; set; }
		
		

    }
}