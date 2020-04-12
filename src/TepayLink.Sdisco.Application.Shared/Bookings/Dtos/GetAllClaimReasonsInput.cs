using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllClaimReasonsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }



    }
}