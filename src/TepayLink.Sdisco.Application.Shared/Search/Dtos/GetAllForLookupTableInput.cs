﻿using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Search.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}