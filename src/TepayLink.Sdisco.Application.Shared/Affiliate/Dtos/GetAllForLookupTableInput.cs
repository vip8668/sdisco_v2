using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Affiliate.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}