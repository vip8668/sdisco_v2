
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Search.Dtos
{
    public class CreateOrEditSearchHistoryDto : EntityDto<long?>
    {

		public long UserId { get; set; }
		
		
		public string Keyword { get; set; }
		
		
		public string Type { get; set; }
		
		

    }
}