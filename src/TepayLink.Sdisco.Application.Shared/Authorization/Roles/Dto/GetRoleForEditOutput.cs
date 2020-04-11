using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Permissions.Dto;

namespace TepayLink.Sdisco.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}