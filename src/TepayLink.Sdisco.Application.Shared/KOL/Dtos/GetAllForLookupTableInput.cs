using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.KOL.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}