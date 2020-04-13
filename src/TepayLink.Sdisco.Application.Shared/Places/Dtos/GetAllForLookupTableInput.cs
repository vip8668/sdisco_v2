using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Places.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}