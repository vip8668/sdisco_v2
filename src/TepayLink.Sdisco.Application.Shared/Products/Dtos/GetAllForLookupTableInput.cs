using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}