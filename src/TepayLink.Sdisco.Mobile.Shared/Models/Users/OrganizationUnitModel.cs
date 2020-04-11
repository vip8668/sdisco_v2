using Abp.AutoMapper;
using TepayLink.Sdisco.Organizations.Dto;

namespace TepayLink.Sdisco.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}