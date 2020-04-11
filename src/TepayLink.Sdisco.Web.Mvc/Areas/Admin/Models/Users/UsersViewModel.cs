using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization.Permissions.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Users
{
    public class UsersViewModel : IPermissionsEditViewModel
    {
        public string FilterText { get; set; }

        public List<ComboboxItemDto> Roles { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}