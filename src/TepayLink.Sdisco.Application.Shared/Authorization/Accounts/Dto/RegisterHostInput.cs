using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Authorization.Accounts.Dto
{
 public   class RegisterHostInput: RegisterInput
    {
        // <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime Dob { get; set; }

        public int CountryId { get; set; }
        public string SubDomain { get; set; }
        public string Occupation { get; set; }
        public string Mobile { get; set; }
    }
}
