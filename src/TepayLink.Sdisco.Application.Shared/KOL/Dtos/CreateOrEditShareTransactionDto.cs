using TepayLink.Sdisco.KOL;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class CreateOrEditShareTransactionDto : EntityDto<long?>
    {

		public long UserId { get; set; }
		
		
		public RevenueTypeEnum Type { get; set; }
		
		
		public string IP { get; set; }
		
		
		public decimal Point { get; set; }
		
		
		public long ProductId { get; set; }
		
		

    }
}