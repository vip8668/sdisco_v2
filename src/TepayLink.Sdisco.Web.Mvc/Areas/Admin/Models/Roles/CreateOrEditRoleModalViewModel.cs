using Abp.AutoMapper;
using TepayLink.Sdisco.Authorization.Roles.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}