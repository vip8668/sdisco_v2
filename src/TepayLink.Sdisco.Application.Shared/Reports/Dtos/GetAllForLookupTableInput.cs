using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Reports.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}