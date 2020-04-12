using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}