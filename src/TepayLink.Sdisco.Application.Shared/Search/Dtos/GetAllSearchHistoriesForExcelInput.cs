using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Search.Dtos
{
    public class GetAllSearchHistoriesForExcelInput
    {
		public string Filter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public string KeywordFilter { get; set; }

		public string TypeFilter { get; set; }



    }
}