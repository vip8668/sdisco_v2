using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Clients.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}