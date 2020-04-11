using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using TepayLink.Sdisco.Authorization.Users.Dto;
using TepayLink.Sdisco.Security;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}