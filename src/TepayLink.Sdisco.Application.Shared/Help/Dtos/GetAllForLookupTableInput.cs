using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Help.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}