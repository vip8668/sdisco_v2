using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}