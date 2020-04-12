using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.AdminConfig.Dtos
{
    public class GetAllCountriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DisplayNameFilter { get; set; }

		public string IconFilter { get; set; }

		public int IsDisabledFilter { get; set; }



    }
}