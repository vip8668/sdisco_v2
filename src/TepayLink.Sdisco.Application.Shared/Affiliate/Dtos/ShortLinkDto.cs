
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Affiliate.Dtos
{
    public class ShortLinkDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public string FullLink { get; set; }

		public string ShortCode { get; set; }



    }
}