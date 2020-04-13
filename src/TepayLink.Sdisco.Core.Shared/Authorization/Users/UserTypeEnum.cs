using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Authorization.Users
{
    public enum UserTypeEnum : byte
    {
        Admin = 0,
        KOL = 1,
        Traveler = 2,
        SupperHost = 3,
        Partner = 4,
    }
}
