using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Permissions.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Common.Modals
{
    public class PermissionTreeModalViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
