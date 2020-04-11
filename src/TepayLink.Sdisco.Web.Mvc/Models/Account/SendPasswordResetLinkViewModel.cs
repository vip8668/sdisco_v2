using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}