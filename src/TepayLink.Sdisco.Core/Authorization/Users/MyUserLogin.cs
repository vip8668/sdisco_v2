using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Authorization.Users
{
    public class MyUserLogin : UserLogin
    {
        public string AccessCode { get; set; }
    }
}
