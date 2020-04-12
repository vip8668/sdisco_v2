using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Cashout.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}