using Abp.AutoMapper;
using Abp.Organizations;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.OrganizationUnits
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class EditOrganizationUnitModalViewModel
    {
        public long? Id { get; set; }

        public string DisplayName { get; set; }
    }
}