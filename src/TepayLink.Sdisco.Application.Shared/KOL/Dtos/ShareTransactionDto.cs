using TepayLink.Sdisco.KOL;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class ShareTransactionDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public RevenueTypeEnum Type { get; set; }

		public string IP { get; set; }

		public decimal Point { get; set; }

		public long ProductId { get; set; }



    }
}