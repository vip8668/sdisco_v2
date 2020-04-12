using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllUserReviewsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}