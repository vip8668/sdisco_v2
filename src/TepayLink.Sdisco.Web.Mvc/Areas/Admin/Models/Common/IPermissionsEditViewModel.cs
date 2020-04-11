using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Permissions.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}