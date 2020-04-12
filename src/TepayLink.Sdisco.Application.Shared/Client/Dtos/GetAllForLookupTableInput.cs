using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Client.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}