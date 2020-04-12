using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Bookings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}