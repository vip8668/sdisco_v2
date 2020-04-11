using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}