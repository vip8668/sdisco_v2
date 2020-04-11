using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Users.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}