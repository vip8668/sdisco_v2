using Abp.AutoMapper;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Authorization.Users.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}