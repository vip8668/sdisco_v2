using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
