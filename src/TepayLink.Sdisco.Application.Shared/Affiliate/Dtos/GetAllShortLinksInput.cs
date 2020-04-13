using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Affiliate.Dtos
{
    public class GetAllShortLinksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}