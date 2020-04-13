using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.KOL;

using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class PartnerRevenueDto : EntityDto<long>
    {
		public long Userid { get; set; }

		public RevenueTypeEnum RevenueType { get; set; }

		public long ProductId { get; set; }

		public decimal Point { get; set; }

		public decimal Money { get; set; }

		public RevenueStatusEnum Status { get; set; }



    }
}