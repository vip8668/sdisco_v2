using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using TepayLink.Sdisco.Validation;

namespace TepayLink.Sdisco.Authorization.Accounts.Dto
{
    public class RegisterInput //: IValidatableObject
    {
        /// <summary>
        /// Tên
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }
        /// <summary>
        /// Họ
        /// </summary>

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        // [Required]
        //   [StringLength(AbpUserBase.MaxUserNameLength)]
        //   private string UserName { get { return this.EmailAddress; } set { } }

        /// <summary>
        /// Email 
        /// </summary>

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Mật khẩu 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }


        [DisableAuditing]
        public string CaptchaResponse { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    //if (!UserName.IsNullOrEmpty())
        //    //{
        //    //    if (!UserName.Equals(EmailAddress, StringComparison.OrdinalIgnoreCase) && ValidationHelper.IsEmail(UserName))
        //    //    {
        //    //        yield return new ValidationResult("Username cannot be an email address unless it's same with your email address !");
        //    //    }
        //    //}
        //    return new 
        //}
    }
}