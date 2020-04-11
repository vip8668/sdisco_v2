using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Permissions.Dto;

namespace TepayLink.Sdisco.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}