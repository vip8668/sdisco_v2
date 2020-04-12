
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Search.Dtos
{
    public class SearchHistoryDto : EntityDto<long>
    {
		public long UserId { get; set; }

		public string Keyword { get; set; }

		public string Type { get; set; }



    }
}