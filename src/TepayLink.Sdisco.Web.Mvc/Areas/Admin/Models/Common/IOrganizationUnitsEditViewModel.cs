using System.Collections.Generic;
using TepayLink.Sdisco.Organizations.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}