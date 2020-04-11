using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}