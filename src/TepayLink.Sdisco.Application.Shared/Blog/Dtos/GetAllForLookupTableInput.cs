using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}