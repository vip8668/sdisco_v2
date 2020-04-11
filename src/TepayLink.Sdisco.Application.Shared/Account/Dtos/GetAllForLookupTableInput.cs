using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Account.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}