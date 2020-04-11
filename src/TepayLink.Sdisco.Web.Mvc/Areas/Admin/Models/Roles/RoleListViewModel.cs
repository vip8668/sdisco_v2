using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization.Permissions.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}